// server.js
const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
const path = require('path');
const session = require('express-session');

const rutasArticulos = require('./routes/articulos');
const rutasAuth = require('./routes/auth');

const app = express();

// Configuración del motor de vistas EJS
app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, 'views/layouts'));

// Middlewares
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use('/uploads', express.static(path.join(__dirname, 'uploads')));

// Configuración de sesiones
app.use(session({
  secret: 'inventario_secret_key_2024',
  resave: false,
  saveUninitialized: false,
  cookie: { maxAge: 1000 * 60 * 60 * 8 } // 8 horas
}));

// Middleware de autenticación: protege todas las rutas de /articulos
function requireLogin(req, res, next) {
  if (req.session.usuario) return next();
  res.redirect('/login');
}

// Rutas públicas (login/logout)
app.use('/', rutasAuth);

// Rutas protegidas
app.use('/articulos', requireLogin, rutasArticulos);

// Conexión a MongoDB
mongoose.connect('mongodb://localhost:27017/inventario')
  .then(() => console.log('Conectado a MongoDB'))
  .catch(err => console.error('Error al conectar a MongoDB:', err));

// Redirigir la raíz al listado de artículos (requireLogin redirige al login si no hay sesión)
app.get('/', requireLogin, (req, res) => {
  res.redirect('/articulos');
});

// Iniciar servidor
app.listen(3000, () => {
  console.log('Servidor ejecutándose en http://localhost:3000');
});
