// routes/auth.js
const express = require('express');
const router = express.Router();
const bcrypt = require('bcryptjs');
const Usuario = require('../models/usuario');

// Mostrar formulario de login
router.get('/login', (req, res) => {
  // Si ya está logueado, redirigir al inventario
  if (req.session.usuario) return res.redirect('/articulos');
  res.render('login', { error: null });
});

// Procesar login
router.post('/login', async (req, res) => {
  const { correo, password } = req.body;
  try {
    const usuario = await Usuario.findOne({ correo });
    if (!usuario) {
      return res.render('login', { error: 'Correo o contraseña incorrectos.' });
    }
    const passwordValida = await bcrypt.compare(password, usuario.password);
    if (!passwordValida) {
      return res.render('login', { error: 'Correo o contraseña incorrectos.' });
    }
    // Guardar usuario en sesión
    req.session.usuario = { id: usuario._id, correo: usuario.correo };
    res.redirect('/articulos');
  } catch (err) {
    console.error(err);
    res.render('login', { error: 'Error al iniciar sesión. Intenta nuevamente.' });
  }
});

// Cerrar sesión
router.get('/logout', (req, res) => {
  req.session.destroy(() => {
    res.redirect('/login');
  });
});

module.exports = router;
