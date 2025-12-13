// ingredients.js

const statusDiv = document.getElementById('connectionStatus');
const contentDiv = document.getElementById('ingredientContent');

// Función para mostrar estado
function showStatus(message, type) {
    statusDiv.innerHTML = `<div class="status ${type}">${message}</div>`;
}

// ==================== READ - Listar Ingredientes ====================

async function loadIngredients() {
    showStatus('⏳ Cargando ingredientes...', 'loading');
    contentDiv.innerHTML = '';
    
    try {
        const ingredients = await fetchIngredients();
        
        if (ingredients.length === 0) {
            contentDiv.innerHTML = '<p>No hay ingredientes disponibles.</p>';
            showStatus('ℹ️ No se encontraron ingredientes en la base de datos.', 'success');
            return;
        }
        
        showStatus(`✅ Se cargaron ${ingredients.length} ingredientes correctamente.`, 'success');
        
        contentDiv.innerHTML = `
            <h3>Ingredientes Disponibles:</h3>
            <div style="background: white; border-radius: 8px; padding: 20px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);">
                <table style="width: 100%; border-collapse: collapse;">
                    <thead>
                        <tr style="background: #f8f9fa; border-bottom: 2px solid #007bff;">
                            <th style="padding: 12px; text-align: left;">Nombre</th>
                            <th style="padding: 12px; text-align: left;">Precio Extra</th>
                            <th style="padding: 12px; text-align: center;">Acciones</th>
                        </tr>
                    </thead>
                    <tbody id="ingredientsTableBody">
                    </tbody>
                </table>
            </div>
        `;
        
        const tbody = document.getElementById('ingredientsTableBody');
        
        ingredients.forEach(ingredient => {
            const row = document.createElement('tr');
            row.style.borderBottom = '1px solid #ddd';
            row.style.transition = 'background-color 0.2s';
            row.onmouseover = () => row.style.backgroundColor = '#f8f9fa';
            row.onmouseout = () => row.style.backgroundColor = 'white';
            
            row.innerHTML = `
                <td style="padding: 12px;">
                    <strong>${ingredient.name}</strong>
                </td>
                <td style="padding: 12px;">
                    <span style="color: #28a745; font-weight: bold;">+$${ingredient.extraPrice.toFixed(2)}</span>
                </td>
                <td style="padding: 12px; text-align: center;">
                    <button onclick="showEditIngredientForm('${ingredient.id}')" 
                            style="background: #ffc107; color: #333; border: none; padding: 6px 12px; border-radius: 4px; cursor: pointer; margin-right: 5px;">
                        ✏️ Editar
                    </button>
                    <button onclick="confirmDeleteIngredient('${ingredient.id}', '${ingredient.name}')" 
                            style="background: #dc3545; color: white; border: none; padding: 6px 12px; border-radius: 4px; cursor: pointer;">
                        🗑️ Eliminar
                    </button>
                </td>
            `;
            
            tbody.appendChild(row);
        });
        
    } catch (error) {
        showStatus(`❌ Error al cargar ingredientes: ${error.message}`, 'error');
        contentDiv.innerHTML = '<p>Error al cargar los ingredientes.</p>';
    }
}

// ==================== CREATE - Crear Ingrediente ====================

function showCreateIngredientForm() {
    contentDiv.innerHTML = `
        <h3>➕ Crear Nuevo Ingrediente</h3>
        <form id="createIngredientForm" style="background: white; padding: 25px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); max-width: 600px;">
            
            <!-- Nombre -->
            <div style="margin-bottom: 20px;">
                <label for="ingredientName" style="display: block; margin-bottom: 5px; font-weight: bold; color: #333;">
                    Nombre del Ingrediente:
                </label>
                <input 
                    type="text" 
                    id="ingredientName" 
                    required 
                    placeholder="Ej: Pepperoni, Champiñones, Aceitunas"
                    style="width: 100%; padding: 10px; border: 2px solid #ddd; border-radius: 6px; font-size: 14px; box-sizing: border-box;"
                >
            </div>
            
            <!-- Precio Extra -->
            <div style="margin-bottom: 20px;">
                <label for="ingredientPrice" style="display: block; margin-bottom: 5px; font-weight: bold; color: #333;">
                    Precio Extra ($):
                </label>
                <input 
                    type="number" 
                    id="ingredientPrice" 
                    step="0.01" 
                    min="0" 
                    required 
                    placeholder="0.50"
                    style="width: 100%; padding: 10px; border: 2px solid #ddd; border-radius: 6px; font-size: 14px; box-sizing: border-box;"
                >
                <small style="display: block; margin-top: 5px; color: #666;">
                    Costo adicional por agregar este ingrediente a una pizza
                </small>
            </div>
            
            <!-- Botones -->
            <div style="display: flex; gap: 10px;">
                <button type="submit" style="flex: 1; padding: 12px; background-color: #28a745;">
                    ✅ Crear Ingrediente
                </button>
                <button type="button" onclick="loadIngredients()" style="flex: 1; padding: 12px; background-color: #6c757d;">
                    ❌ Cancelar
                </button>
            </div>
        </form>
    `;
    
    document.getElementById('createIngredientForm').addEventListener('submit', handleCreateIngredientSubmit);
    showStatus('', '');
}

async function handleCreateIngredientSubmit(e) {
    e.preventDefault();
    
    const name = document.getElementById('ingredientName').value.trim();
    const extraPrice = parseFloat(document.getElementById('ingredientPrice').value);
    
    if (!name || isNaN(extraPrice) || extraPrice < 0) {
        showStatus('❌ Por favor completa todos los campos correctamente', 'error');
        return;
    }
    
    const ingredientData = {
        name: name,
        extraPrice: extraPrice
    };
    
    console.log('📦 Datos a enviar:', JSON.stringify(ingredientData, null, 2));
    
    showStatus('⏳ Creando ingrediente...', 'loading');
    
    try {
        const created = await createIngredient(ingredientData);
        showStatus(`✅ Ingrediente "${created.name}" creado exitosamente!`, 'success');
        setTimeout(() => loadIngredients(), 1500);
    } catch (error) {
        showStatus(`❌ Error al crear ingrediente: ${error.message}`, 'error');
    }
}

// ==================== UPDATE - Editar Ingrediente ====================

async function showEditIngredientForm(id) {
    showStatus('⏳ Cargando ingrediente...', 'loading');
    
    try {
        const ingredient = await getIngredientById(id);
        
        contentDiv.innerHTML = `
            <h3>✏️ Editar Ingrediente</h3>
            <form id="editIngredientForm" style="background: white; padding: 25px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); max-width: 600px;">
                
                <!-- ID oculto -->
                <input type="hidden" id="ingredientId" value="${ingredient.id}">
                
                <!-- Nombre -->
                <div style="margin-bottom: 20px;">
                    <label for="ingredientName" style="display: block; margin-bottom: 5px; font-weight: bold; color: #333;">
                        Nombre del Ingrediente:
                    </label>
                    <input 
                        type="text" 
                        id="ingredientName" 
                        required 
                        value="${ingredient.name}"
                        style="width: 100%; padding: 10px; border: 2px solid #ddd; border-radius: 6px; font-size: 14px; box-sizing: border-box;"
                    >
                </div>
                
                <!-- Precio Extra -->
                <div style="margin-bottom: 20px;">
                    <label for="ingredientPrice" style="display: block; margin-bottom: 5px; font-weight: bold; color: #333;">
                        Precio Extra ($):
                    </label>
                    <input 
                        type="number" 
                        id="ingredientPrice" 
                        step="0.01" 
                        min="0" 
                        required 
                        value="${ingredient.extraPrice}"
                        style="width: 100%; padding: 10px; border: 2px solid #ddd; border-radius: 6px; font-size: 14px; box-sizing: border-box;"
                    >
                </div>
                
                <!-- Botones -->
                <div style="display: flex; gap: 10px;">
                    <button type="submit" style="flex: 1; padding: 12px; background-color: #ffc107; color: #333;">
                        💾 Guardar Cambios
                    </button>
                    <button type="button" onclick="loadIngredients()" style="flex: 1; padding: 12px; background-color: #6c757d;">
                        ❌ Cancelar
                    </button>
                </div>
            </form>
        `;
        
        document.getElementById('editIngredientForm').addEventListener('submit', handleEditIngredientSubmit);
        showStatus('', '');
        
    } catch (error) {
        showStatus(`❌ Error al cargar ingrediente: ${error.message}`, 'error');
    }
}

async function handleEditIngredientSubmit(e) {
    e.preventDefault();
    
    const id = document.getElementById('ingredientId').value;
    const name = document.getElementById('ingredientName').value.trim();
    const extraPrice = parseFloat(document.getElementById('ingredientPrice').value);
    
    if (!name || isNaN(extraPrice) || extraPrice < 0) {
        showStatus('❌ Por favor completa todos los campos correctamente', 'error');
        return;
    }
    
    const ingredientData = {
        name: name,
        extraPrice: extraPrice
    };
    
    console.log('📦 Datos a actualizar:', JSON.stringify(ingredientData, null, 2));
    
    showStatus('⏳ Actualizando ingrediente...', 'loading');
    
    try {
        await updateIngredient(id, ingredientData);
        showStatus(`✅ Ingrediente "${name}" actualizado exitosamente!`, 'success');
        setTimeout(() => loadIngredients(), 1500);
    } catch (error) {
        showStatus(`❌ Error al actualizar ingrediente: ${error.message}`, 'error');
    }
}

// ==================== DELETE - Eliminar Ingrediente ====================

function confirmDeleteIngredient(id, name) {
    if (confirm(`¿Estás seguro de que quieres eliminar "${name}"?\n\nAdvertencia: Si este ingrediente está siendo usado en pizzas, pueden ocurrir errores.`)) {
        handleDeleteIngredient(id, name);
    }
}

async function handleDeleteIngredient(id, name) {
    showStatus('⏳ Eliminando ingrediente...', 'loading');
    
    try {
        await deleteIngredient(id);
        showStatus(`✅ Ingrediente "${name}" eliminado exitosamente!`, 'success');
        setTimeout(() => loadIngredients(), 1000);
    } catch (error) {
        showStatus(`❌ Error al eliminar ingrediente: ${error.message}`, 'error');
    }
}