// ==================== AUTENTICACIÓN ====================

// Construir headers con autenticación
function getAuthHeaders() {
    const token = getToken();
    return {
        'Content-Type': 'application/json',
        ...(token && { 'Authorization': `Bearer ${token}` })
    };
}


// Login de usuario
async function login(email, password) {
    try {
        console.log('🔐 POST:', `${API_URL}/auth/login`);
        
        const response = await fetch(`${API_URL}/auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email, password })
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText || 'Login failed');
        }

        const data = await response.json();
        console.log('✅ Login exitoso:', data);
        
        // Guardar token y datos del usuario
        localStorage.setItem('token', data.token);
        localStorage.setItem('userEmail', data.email);
        localStorage.setItem('userRole', data.role);
        
        return data;
    } catch (error) {
        console.error('❌ Error en login:', error);
        throw error;
    }
}

// Registro de usuario
async function register(email, password) {
    try {
        console.log('📝 POST:', `${API_URL}/auth/register`);
        
        const response = await fetch(`${API_URL}/auth/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email, password })
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText || 'Registration failed');
        }

        const data = await response.json();
        console.log('✅ Registro exitoso:', data);
        
        // Guardar token y datos del usuario
        localStorage.setItem('token', data.token);
        localStorage.setItem('userEmail', data.email);
        localStorage.setItem('userRole', data.role);
        
        return data;
    } catch (error) {
        console.error('❌ Error en registro:', error);
        throw error;
    }
}

// Logout
function logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userEmail');
    localStorage.removeItem('userRole');
    window.location.href = 'login.html';
}

// Verificar si el usuario está autenticado
function isAuthenticated() {
    return localStorage.getItem('token') !== null;
}

// Obtener el rol del usuario
function getUserRole() {
    return localStorage.getItem('userRole');
}

// Obtener el token JWT
function getToken() {
    return localStorage.getItem('token');
}

// Verificar si el usuario es admin
function isAdmin() {
    return getUserRole() === 'Admin';
}

// Proteger páginas - llamar al inicio de cada página
function requireAuth() {
    if (!isAuthenticated()) {
        window.location.href = 'login.html';
        return false;
    }
    return true;
}

// Proteger páginas de admin
function requireAdmin() {
    if (!isAuthenticated()) {
        window.location.href = 'login.html';
        return false;
    }
    
    if (!isAdmin()) {
        alert('❌ Acceso denegado. Solo administradores.');
        logout();
        return false;
    }
    
    return true;
}