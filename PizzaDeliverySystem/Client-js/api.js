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