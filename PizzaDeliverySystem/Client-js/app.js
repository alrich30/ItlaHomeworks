// app.js
const statusDiv = document.getElementById('connectionStatus');
const pizzaListDiv = document.getElementById('pizzaList');

// Función para mostrar estado
function showStatus(message, type) {
    statusDiv.innerHTML = `<div class="status ${type}">${message}</div>`;
}

// Probar conexión básica
async function testConnection() {
    showStatus('⏳ Probando conexión con el servidor...', 'loading');
    
    try {
        const response = await fetch(`${API_URL}/pizza`);
        
        if (response.ok) {
            showStatus('✅ ¡Conexión exitosa! El servidor está respondiendo.', 'success');
            console.log('✅ Headers recibidos:', response.headers);
        } else {
            showStatus(`⚠️ Servidor respondió con error: ${response.status}`, 'error');
        }
    } catch (error) {
        showStatus(`❌ Error de conexión: ${error.message}. ¿Está el backend corriendo?`, 'error');
        console.error('Detalles del error:', error);
    }
}

async function loadPizzas() {
    showStatus('⏳ Cargando pizzas...', 'loading');
    pizzaListDiv.innerHTML = '';
    
    try {
        const pizzas = await fetchPizzas();
        
        if (pizzas.length === 0) {
            pizzaListDiv.innerHTML = '<p>No hay pizzas disponibles.</p>';
            showStatus('ℹ️ No se encontraron pizzas en la base de datos.', 'success');
            return;
        }
        
        showStatus(`✅ Se cargaron ${pizzas.length} pizzas correctamente.`, 'success');
        
        pizzaListDiv.innerHTML = '<h3>Pizzas Disponibles:</h3>';
        
        pizzas.forEach(pizza => {
            const pizzaItem = document.createElement('div');
            pizzaItem.className = 'pizza-item';
            pizzaItem.style.display = 'flex';
            pizzaItem.style.justifyContent = 'space-between';
            pizzaItem.style.alignItems = 'center';
            
            // Calcular precio total
            const precioBase = pizza.basePrice;
            let precioTotal = precioBase;
            if (pizza.ingredients && pizza.ingredients.length > 0) {
                const extraTotal = pizza.ingredients.reduce((sum, ing) => sum + (ing.extraPrice || 0), 0);
                precioTotal += extraTotal;
            }
            
            // Contenido de la pizza
            const pizzaInfo = document.createElement('div');
            pizzaInfo.innerHTML = `
                <strong>${pizza.name}</strong> - Tamaño: ${pizza.size}
                <br>
                <span style="color: #28a745; font-weight: bold;">
                    Precio base: $${precioBase.toFixed(2)} | 
                    Precio total: $${precioTotal.toFixed(2)}
                </span>
                <br>
                <small>
                    🍕 ${pizza.ingredients.length} ingrediente${pizza.ingredients.length !== 1 ? 's' : ''}
                    ${pizza.ingredients.length > 0 
                        ? ': ' + pizza.ingredients.map(ing => ing.name).join(', ')
                        : ''
                    }
                </small>
            `;
            
            // Botón de eliminar
            const deleteBtn = document.createElement('button');
            deleteBtn.textContent = '🗑️ Eliminar';
            deleteBtn.className = 'delete-btn';
            deleteBtn.style.cssText = `
                background-color: #dc3545;
                color: white;
                border: none;
                padding: 8px 15px;
                border-radius: 5px;
                cursor: pointer;
                font-size: 14px;
                margin-left: 10px;
            `;
            
            deleteBtn.onmouseover = () => deleteBtn.style.backgroundColor = '#c82333';
            deleteBtn.onmouseout = () => deleteBtn.style.backgroundColor = '#dc3545';
            
            deleteBtn.onclick = () => confirmDeletePizza(pizza.id, pizza.name);
            
            pizzaItem.appendChild(pizzaInfo);
            pizzaItem.appendChild(deleteBtn);
            pizzaListDiv.appendChild(pizzaItem);
        });
        
    } catch (error) {
        showStatus(`❌ Error al cargar pizzas: ${error.message}`, 'error');
        pizzaListDiv.innerHTML = '<p>Error al cargar las pizzas.</p>';
    }
}

// Confirmar eliminación
function confirmDeletePizza(id, name) {
    if (confirm(`¿Estás seguro de que quieres eliminar "${name}"?`)) {
        handleDeletePizza(id);
    }
}

// Manejar la eliminación
async function handleDeletePizza(id) {
    showStatus('⏳ Eliminando pizza...', 'loading');
    
    try {
        await deletePizza(id);
        showStatus('✅ Pizza eliminada exitosamente!', 'success');
        
        // Recargar la lista después de eliminar
        setTimeout(() => loadPizzas(), 500);
        
    } catch (error) {
        showStatus(`❌ Error al eliminar pizza: ${error.message}`, 'error');
    }
}

// Crear pizza de prueba
async function testCreatePizza() {
    showStatus('⏳ Creando pizza de prueba...', 'loading');
    
    const testPizza = {
        name: 'Pizza de Prueba',
        description: 'Esta es una pizza de prueba creada desde el frontend',
        price: 12.99,
        imageUrl: 'https://via.placeholder.com/150'
    };
    
    try {
        const newPizza = await createPizza(testPizza);
        showStatus('✅ Pizza creada exitosamente!', 'success');
        
        // Recargar la lista
        setTimeout(() => loadPizzas(), 500);
        
    } catch (error) {
        showStatus(`❌ Error al crear pizza: ${error.message}`, 'error');
    }
}


// Variable global para almacenar ingredientes
let availableIngredients = [];

// Mostrar formulario de creación
async function showCreatePizzaForm() {
    showStatus('⏳ Cargando ingredientes...', 'loading');
    
    try {
        // Cargar ingredientes disponibles
        availableIngredients = await fetchIngredients();
        
        pizzaListDiv.innerHTML = `
            <h3>🍕 Crear Nueva Pizza</h3>
            <form id="createPizzaForm" style="background: white; padding: 25px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);">
                
                <!-- Nombre -->
                <div style="margin-bottom: 20px;">
                    <label for="pizzaName" style="display: block; margin-bottom: 5px; font-weight: bold; color: #333;">
                        Nombre de la Pizza:
                    </label>
                    <input 
                        type="text" 
                        id="pizzaName" 
                        required 
                        placeholder="Ej: Pizza Suprema"
                        style="width: 100%; padding: 10px; border: 2px solid #ddd; border-radius: 6px; font-size: 14px;"
                    >
                </div>
                
                <!-- Tamaño -->
                <div style="margin-bottom: 20px;">
                    <label for="pizzaSize" style="display: block; margin-bottom: 5px; font-weight: bold; color: #333;">
                        Tamaño:
                    </label>
                    <select 
                        id="pizzaSize" 
                        required 
                        style="width: 100%; padding: 10px; border: 2px solid #ddd; border-radius: 6px; font-size: 14px;"
                    >
                        <option value="">Selecciona un tamaño</option>
                        <option value="Small">Pequeña (Small)</option>
                        <option value="Medium">Mediana (Medium)</option>
                        <option value="Large">Grande (Large)</option>
                    </select>
                </div>
                
                <!-- Precio Base -->
                <div style="margin-bottom: 20px;">
                    <label for="pizzaPrice" style="display: block; margin-bottom: 5px; font-weight: bold; color: #333;">
                        Precio Base ($):
                    </label>
                    <input 
                        type="number" 
                        id="pizzaPrice" 
                        step="0.01" 
                        min="0" 
                        required 
                        placeholder="15.00"
                        style="width: 100%; padding: 10px; border: 2px solid #ddd; border-radius: 6px; font-size: 14px;"
                    >
                </div>
                
                <!-- Ingredientes -->
                <div style="margin-bottom: 20px;">
                    <label style="display: block; margin-bottom: 10px; font-weight: bold; color: #333;">
                        Ingredientes:
                    </label>
                    <div id="ingredientsList" style="border: 2px solid #ddd; border-radius: 6px; padding: 15px; max-height: 200px; overflow-y: auto; background-color: #f8f9fa;">
                        ${availableIngredients.map(ing => `
                            <div style="margin-bottom: 8px;">
                                <label style="display: flex; align-items: center; cursor: pointer; padding: 5px; border-radius: 4px; transition: background-color 0.2s;">
                                    <input 
                                        type="checkbox" 
                                        name="ingredients" 
                                        value="${ing.id}"
                                        data-name="${ing.name}"
                                        data-price="${ing.extraPrice || 0}"
                                        style="margin-right: 10px; width: 18px; height: 18px; cursor: pointer;"
                                    >
                                    <span style="flex: 1;">${ing.name}</span>
                                    <span style="color: #28a745; font-weight: bold;">+$${(ing.extraPrice || 0).toFixed(2)}</span>
                                </label>
                            </div>
                        `).join('')}
                    </div>
                    <small style="display: block; margin-top: 5px; color: #666;">
                        Selecciona los ingredientes que deseas agregar
                    </small>
                </div>
                
                <!-- Preview del precio total -->
                <div id="pricePreview" style="margin-bottom: 20px; padding: 15px; background-color: #e7f3ff; border-radius: 6px; border-left: 4px solid #007bff;">
                    <strong>Precio Total Estimado:</strong> <span id="totalPrice" style="color: #007bff; font-size: 20px;">$0.00</span>
                </div>
                
                <!-- Botones -->
                <div style="display: flex; gap: 10px;">
                    <button type="submit" style="flex: 1; padding: 12px; background-color: #28a745;">
                        ✅ Crear Pizza
                    </button>
                    <button type="button" onclick="loadPizzas()" style="flex: 1; padding: 12px; background-color: #6c757d;">
                        ❌ Cancelar
                    </button>
                </div>
            </form>
        `;
        
        // Event listeners
        document.getElementById('createPizzaForm').addEventListener('submit', handleCreatePizzaSubmit);
        document.getElementById('pizzaPrice').addEventListener('input', updatePricePreview);
        document.querySelectorAll('input[name="ingredients"]').forEach(checkbox => {
            checkbox.addEventListener('change', updatePricePreview);
        });
        
        showStatus('', ''); // Limpiar mensaje de estado
        
    } catch (error) {
        showStatus(`❌ Error al cargar ingredientes: ${error.message}`, 'error');
        pizzaListDiv.innerHTML = '<p>No se pudieron cargar los ingredientes.</p>';
    }
}

// Actualizar preview del precio
function updatePricePreview() {
    const basePrice = parseFloat(document.getElementById('pizzaPrice').value) || 0;
    const checkboxes = document.querySelectorAll('input[name="ingredients"]:checked');
    
    let extraPrice = 0;
    checkboxes.forEach(checkbox => {
        extraPrice += parseFloat(checkbox.dataset.price) || 0;
    });
    
    const total = basePrice + extraPrice;
    document.getElementById('totalPrice').textContent = `$${total.toFixed(2)}`;
}

// Manejar envío del formulario
async function handleCreatePizzaSubmit(e) {
    e.preventDefault();
    
    // Validación básica
    const name = document.getElementById('pizzaName').value.trim();
    const size = document.getElementById('pizzaSize').value;
    const basePrice = parseFloat(document.getElementById('pizzaPrice').value);
    
    if (!name || !size || isNaN(basePrice) || basePrice <= 0) {
        showStatus('❌ Por favor completa todos los campos correctamente', 'error');
        return;
    }
    
    // Recopilar ingredientes seleccionados
    const selectedIngredients = [];
    const checkboxes = document.querySelectorAll('input[name="ingredients"]:checked');
    
    checkboxes.forEach(checkbox => {
        selectedIngredients.push({
            id: checkbox.value,
            name: checkbox.dataset.name,
            extraPrice: parseFloat(checkbox.dataset.price)
        });
    });
    
    // Construir el objeto según CreatePizzaRequest
    const pizzaData = {
        name: name,
        size: size,
        basePrice: basePrice,
        ingredients: selectedIngredients
    };
    
    console.log('📦 Datos a enviar:', JSON.stringify(pizzaData, null, 2));
    
    showStatus('⏳ Creando pizza...', 'loading');
    
    try {
        const created = await createPizza(pizzaData);
        showStatus(`✅ Pizza "${created.name}" creada exitosamente!`, 'success');
        setTimeout(() => loadPizzas(), 1500);
    } catch (error) {
        showStatus(`❌ Error al crear pizza: ${error.message}`, 'error');
    }
}

// Probar conexión al cargar la página
window.addEventListener('DOMContentLoaded', () => {
    console.log('🚀 Aplicación cargada');
    console.log('📡 API URL configurada:', API_URL);
    testConnection();
});