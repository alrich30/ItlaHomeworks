// models/articulo.js
const mongoose = require('mongoose');

const ArticuloSchema = new mongoose.Schema({
  codigo: String,
  nombre: String,
  foto: String,
  descripcion: String,
  cantidad: Number,
  precio: Number
});

module.exports = mongoose.model('Articulo', ArticuloSchema);
