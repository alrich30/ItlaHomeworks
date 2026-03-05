const express = require('express');
const router = express.Router();
const Articulo = require('../models/articulo');
const multer = require('multer');
const path = require('path');

// Configurar multer para guardar las imágenes
const storage = multer.diskStorage({
  destination: function (req, file, cb) {
    cb(null, 'uploads/') // Carpeta donde se guardarán las imágenes
  },
  filename: function (req, file, cb) {
    const uniqueSuffix = Date.now() + '-' + Math.round(Math.random() * 1E9);
    cb(null, uniqueSuffix + path.extname(file.originalname));
  }
});

const upload = multer({ storage: storage });

// Mostrar la vista principal con la lista de artículos
router.get('/', async (req, res) => {
  try {
    const articulos = await Articulo.find();
    res.render('index', { articulos });
  } catch (err) {
    res.status(500).send('Error al cargar los artículos');
  }
});

// Mostrar el formulario para agregar un nuevo artículo
router.get('/nuevo', (req, res) => {
  res.render('addArticle');
});

// Crear artículo desde formulario
router.post('/', upload.single('foto'), async (req, res) => {
  try {
    console.log('Body:', req.body); // Ahora sí verás los datos
    console.log('File:', req.file);  // Info del archivo subido
    
    const datosArticulo = {
      codigo: req.body.codigo,
      nombre: req.body.nombre,
      descripcion: req.body.descripcion,
      cantidad: req.body.cantidad,
      precio: req.body.precio,
      foto: req.file ? req.file.filename : null // Guardar nombre del archivo
    };
    
    const nuevoArticulo = new Articulo(datosArticulo);
    await nuevoArticulo.save();
    res.redirect('/articulos');
  } catch (err) {
    console.error('Error:', err);
    res.status(500).send('Error al guardar el artículo');
  }
});



// Mostrar formulario de edición
router.get('/:id/editar', async (req, res) => {
  try {
    const articulo = await Articulo.findById(req.params.id);
    res.render('editArticle', { articulo });
  } catch (err) {
    console.error(err);
    res.status(500).send('Error al cargar el artículo');
  }
});

// Actualizar un artículo (desde formulario)
router.post('/:id/editar', upload.single('foto'), async (req, res) => {
  try {
    console.log('Body:', req.body);
    console.log('File:', req.file);
    
    const datosActualizados = {
      codigo: req.body.codigo,
      nombre: req.body.nombre,
      descripcion: req.body.descripcion,
      cantidad: req.body.cantidad,
      precio: req.body.precio
    };
    
    // Si se subió una nueva foto, actualizarla
    if (req.file) {
      datosActualizados.foto = req.file.filename;
    }
    
    await Articulo.findByIdAndUpdate(req.params.id, datosActualizados, { new: true });
    res.redirect('/articulos');
  } catch (err) {
    console.error('Error:', err);
    res.status(500).send('Error al actualizar el artículo');
  }
});

// Eliminar un artículo (desde formulario)
router.post('/:id/eliminar', async (req, res) => {
  try {
    await Articulo.findByIdAndDelete(req.params.id);
    res.redirect('/articulos');
  } catch (err) {
    console.error('Error:', err);
    res.status(500).send('Error al eliminar el artículo');
  }
});

module.exports = router;
