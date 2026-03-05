const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
const path = require('path');
const rutasArticulos = require('./routes/articulos');

const app = express();

// Configuración del motor de vistas EJS
app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, 'views/layouts'));

// Middlewares
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true })); // NECESARIO para formularios

app.use('/uploads', express.static(path.join(__dirname, 'uploads')));


// Usar las rutas de artículos
app.use('/articulos', rutasArticulos);

// Conexión a MongoDB
mongoose.connect('mongodb://localhost:27017/inventario')
  .then(() => console.log('Conectado a MongoDB'))
  .catch(err => console.error('Error al conectar a MongoDB:', err));

// Redirigir la raíz al listado de artículos
app.get('/', (req, res) => {
  res.redirect('/articulos');
});

// Iniciar servidor
app.listen(3000, () => {
  console.log('Servidor ejecutándose en http://localhost:3000');
});
