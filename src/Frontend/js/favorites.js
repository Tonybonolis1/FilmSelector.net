// Configuración
const API_BASE_URL = 'http://localhost:5001/api';

// Estado
let favorites = [];
let editingFavoriteId = null;
let searchedMovies = [];

// Referencias al DOM
const userIcon = document.getElementById('userIcon');
const userName = document.getElementById('userName');
const userEmail = document.getElementById('userEmail');
const messageContainer = document.getElementById('messageContainer');
const loadingContainer = document.getElementById('loadingContainer');
const favoritesGrid = document.getElementById('favoritesGrid');
const emptyState = document.getElementById('emptyState');
const favoriteModal = document.getElementById('favoriteModal');
const favoriteForm = document.getElementById('favoriteForm');
const modalTitle = document.getElementById('modalTitle');
const searchSection = document.getElementById('searchSection');
const searchResults = document.getElementById('searchResults');
const moviesList = document.getElementById('moviesList');
const saveBtn = document.getElementById('saveBtn');

// Verificar autenticación
function checkAuth() {
    const token = localStorage.getItem('authToken');
    const username = localStorage.getItem('username');
    const email = localStorage.getItem('email');
    
    if (!token || !username) {
        window.location.href = 'login.html';
        return false;
    }
    
    // Actualizar UI con datos del usuario
    userName.textContent = username;
    userEmail.textContent = email || '';
    userIcon.textContent = username.charAt(0).toUpperCase();
    
    return token;
}

// Obtener headers con autenticación
function getAuthHeaders() {
    const token = localStorage.getItem('authToken');
    return {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
    };
}

// Cargar favoritos
async function loadFavorites() {
    showLoading(true);
    clearMessage();
    
    try {
        const response = await fetch(`${API_BASE_URL}/favorites`, {
            headers: getAuthHeaders()
        });
        
        if (response.status === 401) {
            logout();
            return;
        }
        
        if (!response.ok) {
            throw new Error('Error al cargar favoritos');
        }
        
        favorites = await response.json();
        renderFavorites();
        
    } catch (error) {
        console.error('Error:', error);
        showMessage('Error al cargar los favoritos', 'error');
    } finally {
        showLoading(false);
    }
}

// Renderizar favoritos
function renderFavorites() {
    if (favorites.length === 0) {
        favoritesGrid.style.display = 'none';
        emptyState.style.display = 'block';
        return;
    }
    
    favoritesGrid.style.display = 'grid';
    emptyState.style.display = 'none';
    
    favoritesGrid.innerHTML = favorites.map(favorite => `
        <div class="favorite-card">
            <img src="${favorite.poster && favorite.poster !== 'N/A' ? escapeHtml(favorite.poster) : 'https://via.placeholder.com/300x450?text=Sin+Póster'}" 
                 alt="${escapeHtml(favorite.movieTitle)}" 
                 style="width: 100%; height: 300px; object-fit: cover; border-radius: 10px; margin-bottom: 15px;"
                 onerror="this.src='https://via.placeholder.com/300x450?text=Sin+Póster'">
            <h3>🎬 ${escapeHtml(favorite.movieTitle)}</h3>
            <div class="favorite-info">
                <div class="favorite-info-item">
                    <span class="favorite-info-label">Año:</span>
                    <span class="favorite-info-value">${escapeHtml(favorite.year)}</span>
                </div>
                <div class="favorite-info-item">
                    <span class="favorite-info-label">Tipo:</span>
                    <span class="favorite-info-value">${escapeHtml(favorite.type)}</span>
                </div>
                <div class="favorite-info-item">
                    <span class="favorite-info-label">IMDB ID:</span>
                    <span class="favorite-info-value">${escapeHtml(favorite.imdbId)}</span>
                </div>
            </div>
            ${favorite.notes ? `
            <div class="favorite-notes">
                📝 ${escapeHtml(favorite.notes)}
            </div>
            ` : ''}
            <div class="favorite-actions">
                <button class="btn btn-primary btn-small" onclick="editFavorite(${favorite.id})">✏️ Editar</button>
                <button class="btn btn-danger btn-small" onclick="deleteFavorite(${favorite.id})">🗑️ Eliminar</button>
            </div>
        </div>
    `).join('');
}

// Abrir modal para agregar
function openAddModal() {
    editingFavoriteId = null;
    modalTitle.textContent = 'Agregar Película Favorita';
    favoriteForm.reset();
    document.getElementById('favoriteId').value = '';
    document.getElementById('imdbId').value = '';
    document.getElementById('moviePoster').value = '';
    document.getElementById('movieYear').value = '';
    document.getElementById('movieType').value = '';
    document.getElementById('movieTitle').value = '';
    document.getElementById('movieTitle').removeAttribute('readonly');
    document.getElementById('movieTitle').style.background = '';
    searchSection.style.display = 'block';
    searchResults.style.display = 'none';
    saveBtn.disabled = true;
    favoriteModal.classList.add('active');
}

// Buscar películas
async function searchMovies() {
    const searchTitle = document.getElementById('searchMovieTitle').value.trim();
    
    if (!searchTitle) {
        showMessage('Por favor ingresa un título para buscar', 'error');
        return;
    }
    
    try {
        const response = await fetch(`${API_BASE_URL}/movies/search?title=${encodeURIComponent(searchTitle)}`);
        
        if (!response.ok) {
            throw new Error('Error al buscar películas');
        }
        
        const movies = await response.json();
        
        if (movies.length === 0) {
            moviesList.innerHTML = '<p style="text-align: center; color: #666;">No se encontraron películas</p>';
            searchResults.style.display = 'block';
            return;
        }
        
        searchedMovies = movies;
        renderMovieResults(movies);
        
    } catch (error) {
        console.error('Error:', error);
        showMessage('Error al buscar películas', 'error');
    }
}

// Renderizar resultados de búsqueda
function renderMovieResults(movies) {
    moviesList.innerHTML = movies.map((movie, index) => `
        <div style="display: flex; gap: 10px; padding: 10px; border: 1px solid #ddd; border-radius: 5px; margin-bottom: 10px; cursor: pointer; transition: background 0.2s;" 
             onclick="selectMovie(${index})"
             onmouseover="this.style.background='#f0f0f0'"
             onmouseout="this.style.background='white'">
            <img src="${movie.poster && movie.poster !== 'N/A' ? movie.poster : 'https://via.placeholder.com/50x75?text=N/A'}" 
                 alt="${movie.title}" 
                 style="width: 50px; height: 75px; object-fit: cover; border-radius: 3px;">
            <div style="flex: 1;">
                <strong>${escapeHtml(movie.title)}</strong><br>
                <small>Año: ${escapeHtml(movie.year)} | Tipo: ${escapeHtml(movie.type)}</small><br>
                <small style="color: #666;">IMDB: ${escapeHtml(movie.imdbId)}</small>
            </div>
        </div>
    `).join('');
    
    searchResults.style.display = 'block';
}

// Seleccionar película
function selectMovie(index) {
    const movie = searchedMovies[index];
    
    document.getElementById('movieTitle').value = movie.title;
    document.getElementById('imdbId').value = movie.imdbId;
    document.getElementById('moviePoster').value = movie.poster || '';
    document.getElementById('movieYear').value = movie.year;
    document.getElementById('movieType').value = movie.type;
    document.getElementById('movieTitle').setAttribute('readonly', true);
    document.getElementById('movieTitle').style.background = '#f0f0f0';
    
    saveBtn.disabled = false;
    searchResults.style.display = 'none';
    
    showMessage(`Película seleccionada: ${movie.title}`, 'success');
}

// Editar favorito
function editFavorite(id) {
    const favorite = favorites.find(f => f.id === id);
    if (!favorite) return;
    
    editingFavoriteId = id;
    modalTitle.textContent = 'Editar Película Favorita';
    
    document.getElementById('favoriteId').value = favorite.id;
    document.getElementById('movieTitle').value = favorite.movieTitle;
    document.getElementById('imdbId').value = favorite.imdbId;
    document.getElementById('moviePoster').value = favorite.poster || '';
    document.getElementById('movieYear').value = favorite.year;
    document.getElementById('movieType').value = favorite.type;
    document.getElementById('notes').value = favorite.notes || '';
    
    // En modo edición, ocultar búsqueda y habilitar solo edición de título y notas
    searchSection.style.display = 'none';
    document.getElementById('movieTitle').removeAttribute('readonly');
    document.getElementById('movieTitle').style.background = '';
    saveBtn.disabled = false;
    
    favoriteModal.classList.add('active');
}

// Cerrar modal
function closeModal() {
    favoriteModal.classList.remove('active');
    favoriteForm.reset();
    editingFavoriteId = null;
}

// Manejar envío del formulario
favoriteForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    
    const movieTitle = document.getElementById('movieTitle').value.trim();
    const imdbId = document.getElementById('imdbId').value.trim();
    const poster = document.getElementById('moviePoster').value.trim();
    const year = document.getElementById('movieYear').value.trim();
    const type = document.getElementById('movieType').value.trim();
    const notes = document.getElementById('notes').value.trim();
    
    if (!movieTitle || !imdbId) {
        showMessage('Por favor selecciona una película de la búsqueda', 'error');
        return;
    }
    
    const submitBtn = favoriteForm.querySelector('button[type="submit"]');
    const originalText = submitBtn.textContent;
    submitBtn.disabled = true;
    submitBtn.textContent = 'Guardando...';
    
    try {
        let response;
        
        if (editingFavoriteId) {
            // Actualizar
            response = await fetch(`${API_BASE_URL}/favorites/${editingFavoriteId}`, {
                method: 'PUT',
                headers: getAuthHeaders(),
                body: JSON.stringify({
                    movieTitle,
                    notes: notes || null
                })
            });
        } else {
            // Crear
            response = await fetch(`${API_BASE_URL}/favorites`, {
                method: 'POST',
                headers: getAuthHeaders(),
                body: JSON.stringify({
                    movieTitle,
                    imdbId,
                    year,
                    type,
                    poster: poster || null,
                    notes: notes || null
                })
            });
        }
        
        if (response.status === 401) {
            logout();
            return;
        }
        
        const data = await response.json();
        
        if (!response.ok) {
            throw new Error(data.message || 'Error al guardar el favorito');
        }
        
        showMessage(
            editingFavoriteId ? 'Película actualizada correctamente' : 'Película agregada a favoritos',
            'success'
        );
        
        closeModal();
        await loadFavorites();
        
    } catch (error) {
        console.error('Error:', error);
        showMessage(error.message, 'error');
    } finally {
        submitBtn.disabled = false;
        submitBtn.textContent = originalText;
    }
});

// Eliminar favorito
async function deleteFavorite(id) {
    if (!confirm('¿Estás seguro de que deseas eliminar este favorito?')) {
        return;
    }
    
    try {
        const response = await fetch(`${API_BASE_URL}/favorites/${id}`, {
            method: 'DELETE',
            headers: getAuthHeaders()
        });
        
        if (response.status === 401) {
            logout();
            return;
        }
        
        if (!response.ok) {
            const data = await response.json();
            throw new Error(data.message || 'Error al eliminar el favorito');
        }
        
        showMessage('Favorito eliminado correctamente', 'success');
        await loadFavorites();
        
    } catch (error) {
        console.error('Error:', error);
        showMessage(error.message, 'error');
    }
}

// Cerrar sesión
function logout() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('username');
    localStorage.removeItem('email');
    window.location.href = 'login.html';
}

// Ir a búsqueda
function goToSearch() {
    window.location.href = 'index.html';
}

// Mostrar/ocultar loading
function showLoading(show) {
    loadingContainer.style.display = show ? 'block' : 'none';
}

// Mostrar mensajes
function showMessage(message, type) {
    messageContainer.innerHTML = `<div class="message ${type}">${escapeHtml(message)}</div>`;
    
    setTimeout(() => {
        messageContainer.innerHTML = '';
    }, 5000);
}

// Limpiar mensajes
function clearMessage() {
    messageContainer.innerHTML = '';
}

// Escapar HTML
function escapeHtml(text) {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Cerrar modal al hacer clic fuera
favoriteModal.addEventListener('click', (e) => {
    if (e.target === favoriteModal) {
        closeModal();
    }
});

// Inicializar
const token = checkAuth();
if (token) {
    loadFavorites();
}
