// Genera un color RGB aleatorio
function colorAleatorio() {
	const r = Math.floor(Math.random() * 256);
	const g = Math.floor(Math.random() * 256);
	const b = Math.floor(Math.random() * 256);
	return `rgb(${r},${g},${b})`;
}

// Selecciona el div y el botón
const caja = document.getElementById('caja');
const boton = document.querySelector('button');

// Evento para cambiar el color
boton.addEventListener('click', () => {
	caja.style.backgroundColor = colorAleatorio();
	console.log('Color cambiado correctamente');
});
