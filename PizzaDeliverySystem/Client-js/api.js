




// Obtener todas las pizzas
async function fetchPizzas() {
    try {
        console.log('🚀 GET:', `${API_URL}/pizza`);
        const response = await fetch(`${API_URL}/pizza`);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        console.log('✅ Pizzas recibidas:', data);
        return data;
    } catch (error) {
        console.error('❌ Error obteniendo pizzas:', error);
        throw error;
    }
}



// Crear pizza
async function createPizza(pizzaData) {
    try {
        console.log('🚀 POST:', `${API_URL}/pizza`, pizzaData);
        
        const response = await fetch(`${API_URL}/pizza`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(pizzaData)
        });
        
        console.log('📡 Respuesta:', response.status);
        
        if (!response.ok) {
            const errorText = await response.text();
            console.error('Error del servidor:', errorText);
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        console.log('✅ Pizza creada:', data);
        return data;
    } catch (error) {
        console.error('❌ Error creando pizza:', error);
        throw error;
    }
}

// Eliminar pizza por ID
async function deletePizza(id) {
    try {
        console.log('🗑️ DELETE:', `${API_URL}/pizza/${id}`);
        
        const response = await fetch(`${API_URL}/pizza/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            }
        });

        console.log('📡 Respuesta:', response.status);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        // Algunas APIs retornan 204 (No Content) al eliminar
        if (response.status === 204) {
            console.log('✅ Pizza eliminada (204 No Content)');
            return { success: true };
        }

        const data = await response.json();
        console.log('✅ Pizza eliminada:', data);
        return data;

    } catch (error) {
        console.error('❌ Error eliminando pizza:', error);
        throw error;
    }
}

// ==================== INGREDIENTES ====================


// Obtener todos los ingredientes disponibles
async function fetchIngredients() {
    try {
        console.log('🚀 GET:', `${API_URL}/ingredient`);
        const response = await fetch(`${API_URL}/ingredient`);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        console.log('✅ Ingredientes recibidos:', data);
        return data;
    } catch (error) {
        console.error('❌ Error obteniendo ingredientes:', error);
        throw error;
    }
}

// Obtener todos los ingredientes
async function fetchIngredients() {
   try {
        console.log('🚀 GET:', `${API_URL}/ingredient`);
        const response = await fetch(`${API_URL}/ingredient`, {
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        console.log('✅ Ingredientes recibidos:', data);
        return data;
    } catch (error) {
        console.error('❌ Error obteniendo ingredientes:', error);
        throw error;
    }
}

// Obtener ingrediente por ID
async function getIngredientById(id) {
    try {
        console.log('🚀 GET:', `${API_URL}/ingredient/${id}`);
        const response = await fetch(`${API_URL}/ingredient/${id}`, {
            headers: getAuthHeaders()
        });
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        console.log('✅ Ingrediente recibido:', data);
        return data;
    } catch (error) {
        console.error('❌ Error obteniendo ingrediente:', error);
        throw error;
    }
}

// Crear ingrediente
async function createIngredient(ingredientData) {
    try {
        console.log('🚀 POST:', `${API_URL}/ingredient`, ingredientData);
        
        const response = await fetch(`${API_URL}/ingredient`, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(ingredientData)
        });
        
        console.log('📡 Respuesta:', response.status);
        
        if (!response.ok) {
            const errorText = await response.text();
            console.error('Error del servidor:', errorText);
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        console.log('✅ Ingrediente creado:', data);
        return data;
    } catch (error) {
        console.error('❌ Error creando ingrediente:', error);
        throw error;
    }
}

// Actualizar ingrediente
async function updateIngredient(id, ingredientData) {
    try {
        console.log('🚀 PUT:', `${API_URL}/ingredient/${id}`, ingredientData);
        
        const response = await fetch(`${API_URL}/ingredient/${id}`, {
            method: 'PUT',
            headers: getAuthHeaders(),
            body: JSON.stringify(ingredientData)
        });
        
        console.log('📡 Respuesta:', response.status);
        
        if (!response.ok) {
            const errorText = await response.text();
            console.error('Error del servidor:', errorText);
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        // Algunos endpoints PUT retornan 204 No Content
        if (response.status === 204) {
            console.log('✅ Ingrediente actualizado (204 No Content)');
            return { success: true };
        }
        
        const data = await response.json();
        console.log('✅ Ingrediente actualizado:', data);
        return data;
    } catch (error) {
        console.error('❌ Error actualizando ingrediente:', error);
        throw error;
    }
}

// Eliminar ingrediente
async function deleteIngredient(id) {
    try {
        console.log('🗑️ DELETE:', `${API_URL}/ingredient/${id}`);
        
        const response = await fetch(`${API_URL}/ingredient/${id}`, {
            method: 'DELETE',
            headers: getAuthHeaders()
        });

        console.log('📡 Respuesta:', response.status);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        if (response.status === 204) {
            console.log('✅ Ingrediente eliminado (204 No Content)');
            return { success: true };
        }

        const data = await response.json();
        console.log('✅ Ingrediente eliminado:', data);
        return data;

    } catch (error) {
        console.error('❌ Error eliminando ingrediente:', error);
        throw error;
    }
}