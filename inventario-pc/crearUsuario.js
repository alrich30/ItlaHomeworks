// crearUsuario.js
// Ejecutar UNA SOLA VEZ para crear el usuario administrador:
//   node crearUsuario.js

const mongoose = require('mongoose');
const bcrypt = require('bcryptjs');
const Usuario = require('./models/usuario');

const CORREO   = 'admin@inventario.com';
const PASSWORD = 'admin123';

mongoose.connect('mongodb://localhost:27017/inventario')
  .then(async () => {
    const existe = await Usuario.findOne({ correo: CORREO });
    if (existe) {
      console.log('El usuario ya existe:', CORREO);
      return mongoose.disconnect();
    }
    const hash = await bcrypt.hash(PASSWORD, 10);
    await Usuario.create({ correo: CORREO, password: hash });
    console.log('Usuario creado exitosamente:');
    console.log('  Correo:    ', CORREO);
    console.log('  Contraseña:', PASSWORD);
    mongoose.disconnect();
  })
  .catch(err => {
    console.error('Error:', err);
    mongoose.disconnect();
  });
