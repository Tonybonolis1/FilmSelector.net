// ========================================
// CONFIGURACIÓN Y CONSTANTES
// ========================================
const API_BASE_URL = 'http://localhost:5001/api';

// Estado global
let userFavorites = [];

// ========================================
// REFERENCIAS AL DOM
// ========================================
const searchForm = document.getElementById('searchForm');
const searchTitleInput = document.getElementById('searchTitle');
const messageContainer = document.getElementById('messageContainer');
const resultsSection = document.getElementById('resultsSection');
const moviesGrid = document.getElementById('moviesGrid');
const detailsSection = document.getElementById('detailsSection');
const movieDetails = document.getElementById('movieDetails');
const closeDetailsBtn = document.getElementById('closeDetailsBtn');
const authButtons = document.getElementById('authButtons');

// ========================================
// AUTENTICACIÓN
// ========================================
function checkAuthStatus() {
    const token = localStorage.getItem('authToken');
    const username = localStorage.getItem('username');
    
    if (token && username) {
        // Usuario logueado
        authButtons.innerHTML = `
            <span style="margin-right: 10px; color: #2c3e50;">👋 Hola, ${escapeHtml(username)}</span>
            <button onclick="goToFavorites()" class="btn btn-primary" style="padding: 8px 16px;">⭐ Mis Favoritos</button>
            <button onclick="logout()" class="btn btn-secondary" style="padding: 8px 16px;">Cerrar Sesión</button>
        `;
    } else {
        // Usuario no logueado
        authButtons.innerHTML = `
            <button onclick="goToLogin()" class="btn btn-primary" style="padding: 8px 16px;">🔐 Iniciar Sesión</button>
        `;
    }
}

function goToLogin() {
    window.location.href = 'login.html';
}

function goToFavorites() {
    window.location.href = 'favorites.html';
}

function logout() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('username');
    localStorage.removeItem('email');
    showMessage('Sesión cerrada correctamente', 'success');
    checkAuthStatus();
}

// ========================================
// EVENT LISTENERS
// ========================================
searchForm.addEventListener('submit', handleSearch);
closeDetailsBtn.addEventListener('click', handleCloseDetails);

// Verificar estado de autenticación al cargar
document.addEventListener('DOMContentLoaded', () => {
    checkAuthStatus();
    loadUserFavorites();
});

// ========================================
// FUNCIONES DE FAVORITOS
// ========================================
async function loadUserFavorites() {
    const token = localStorage.getItem('authToken');
    if (!token) {
        userFavorites = [];
        return;
    }

    try {
        const response = await fetch(`${API_BASE_URL}/favorites`, {
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            userFavorites = await response.json();
        } else {
            userFavorites = [];
        }
    } catch (error) {
        console.error('Error al cargar favoritos:', error);
        userFavorites = [];
    }
}

function isMovieInFavorites(imdbId) {
    return userFavorites.some(fav => fav.imdbId === imdbId);
}

async function addToFavorites(movie) {
    const token = localStorage.getItem('authToken');
    
    if (!token) {
        showMessage('Debes iniciar sesión para agregar favoritos', 'error');
        setTimeout(() => goToLogin(), 2000);
        return;
    }

    try {
        const response = await fetch(`${API_BASE_URL}/favorites`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                movieTitle: movie.title,
                imdbId: movie.imdbId,
                year: movie.year,
                type: movie.type,
                poster: movie.poster,
                notes: ''
            })
        });

        if (response.status === 401) {
            showMessage('Tu sesión ha expirado. Por favor inicia sesión nuevamente.', 'error');
            setTimeout(() => goToLogin(), 2000);
            return;
        }

        if (!response.ok) {
            const errorData = await response.json().catch(() => ({}));
            throw new Error(errorData.message || 'Error al agregar favorito');
        }

        showMessage(`"${movie.title}" agregada a favoritos! ⭐`, 'success');
        await loadUserFavorites();
        
        // Actualizar tarjetas en pantalla
        displayMovieResults(Array.from(document.querySelectorAll('.movie-card')).map(card => {
            const imdbId = card.getAttribute('data-imdb-id');
            const title = card.querySelector('.movie-title').textContent;
            const year = card.querySelector('.movie-year').textContent;
            const type = card.querySelector('.movie-type').textContent;
            const poster = card.querySelector('.movie-poster').src;
            return { imdbId, title, year, type, poster };
        }));
        
    } catch (error) {
        console.error('Error:', error);
        showMessage(`Error: ${error.message}`, 'error');
    }
}


// ========================================
// FUNCIONES DE BÚSQUEDA
// ========================================
async function handleSearch(e) {
    e.preventDefault();
    
    const title = searchTitleInput.value.trim();
    
    if (!title) {
        showMessage('Por favor ingresa un título de película', 'error');
        return;
    }

    // Limpiar resultados anteriores
    clearMessages();
    hideSection(resultsSection);
    hideSection(detailsSection);
    
    // Mostrar estado de carga
    setButtonLoading(searchForm.querySelector('.btn-primary'), true);

    try {
        const response = await fetch(`${API_BASE_URL}/movies/search?title=${encodeURIComponent(title)}`);
        
        if (!response.ok) {
            const errorData = await response.json().catch(() => ({}));
            throw new Error(errorData.message || `Error ${response.status}: ${response.statusText}`);
        }

        const movies = await response.json();

        if (movies.length === 0) {
            showMessage('No se encontraron películas con ese título. Intenta con otro nombre.', 'warning');
            return;
        }

        displayMovieResults(movies);
        showMessage(`Se encontraron ${movies.length} película(s)`, 'success');
        
    } catch (error) {
        console.error('Error al buscar películas:', error);
        showMessage(`Error: ${error.message}`, 'error');
    } finally {
        setButtonLoading(searchForm.querySelector('.btn-primary'), false);
    }
}

// ========================================
// FUNCIONES DE VISUALIZACIÓN
// ========================================
function displayMovieResults(movies) {
    moviesGrid.innerHTML = '';

    movies.forEach(movie => {
        const movieCard = createMovieCard(movie);
        moviesGrid.appendChild(movieCard);
    });

    showSection(resultsSection);
}

function createMovieCard(movie) {
    const card = document.createElement('div');
    card.className = 'movie-card';
    card.setAttribute('data-imdb-id', movie.imdbId);

    const posterUrl = movie.poster && movie.poster !== 'N/A' 
        ? movie.poster 
        : 'https://via.placeholder.com/300x450?text=Sin+Póster';

    const isFavorite = isMovieInFavorites(movie.imdbId);
    const token = localStorage.getItem('authToken');

    card.innerHTML = `
        <img src="${posterUrl}" alt="${escapeHtml(movie.title)}" class="movie-poster" 
             onerror="this.src='https://via.placeholder.com/300x450?text=Sin+Póster'"
             onclick="loadMovieDetails('${movie.imdbId}')">
        <div class="movie-info">
            <div class="movie-title" title="${escapeHtml(movie.title)}">${escapeHtml(movie.title)}</div>
            <div class="movie-year">${escapeHtml(movie.year)}</div>
            <span class="movie-type">${escapeHtml(movie.type)}</span>
        </div>
        ${isFavorite ? '<div class="favorite-badge">⭐ En Favoritos</div>' : ''}
        ${token && !isFavorite ? `
            <button class="btn-add-favorite" onclick="event.stopPropagation(); addToFavorites({
                title: '${escapeHtml(movie.title).replace(/'/g, "\\'")}',
                imdbId: '${movie.imdbId}',
                year: '${escapeHtml(movie.year)}',
                type: '${escapeHtml(movie.type)}',
                poster: '${posterUrl.replace(/'/g, "\\'")}'
            })" title="Agregar a favoritos">
                ⭐ Agregar
            </button>
        ` : ''}
    `;

    return card;
}

async function loadMovieDetails(imdbId) {
    clearMessages();
    hideSection(resultsSection);
    hideSection(detailsSection);

    try {
        const response = await fetch(`${API_BASE_URL}/movies/${encodeURIComponent(imdbId)}`);
        
        if (!response.ok) {
            const errorData = await response.json().catch(() => ({}));
            throw new Error(errorData.message || `Error ${response.status}: ${response.statusText}`);
        }

        const movie = await response.json();
        displayMovieDetails(movie);
        
    } catch (error) {
        console.error('Error al cargar detalles:', error);
        showMessage(`Error al cargar detalles: ${error.message}`, 'error');
        showSection(resultsSection);
    }
}

function displayMovieDetails(movie) {
    const posterUrl = movie.poster && movie.poster !== 'N/A' 
        ? movie.poster 
        : 'https://via.placeholder.com/300x450?text=Sin+Póster';

    const ratingClass = movie.isHighlyRated ? 'movie-rating-high' : '';
    const ratingEmoji = movie.isHighlyRated ? '⭐' : '🎬';

    movieDetails.innerHTML = `
        <div>
            <img src="${posterUrl}" alt="${escapeHtml(movie.title)}" class="movie-details-poster"
                 onerror="this.src='https://via.placeholder.com/300x450?text=Sin+Póster'">
        </div>
        <div class="movie-details-info">
            <h3 class="movie-details-title">${escapeHtml(movie.title)} (${escapeHtml(movie.year)})</h3>
            
            <div class="movie-details-meta">
                <span>📅 ${escapeHtml(movie.released)}</span>
                <span>⏱️ ${escapeHtml(movie.runtime)}</span>
                <span>🎭 ${escapeHtml(movie.rated)}</span>
                <span class="movie-rating ${ratingClass}">
                    ${ratingEmoji} IMDb: ${escapeHtml(movie.imdbRating)} (${formatVotes(movie.imdbVotes)} votos)
                </span>
            </div>

            <p class="movie-plot"><strong>Sinopsis:</strong> ${escapeHtml(movie.plot)}</p>

            <div class="movie-details-grid">
                <div class="movie-detail-item">
                    <strong>Género</strong>
                    ${escapeHtml(movie.genre)}
                </div>
                <div class="movie-detail-item">
                    <strong>Director</strong>
                    ${escapeHtml(movie.director)}
                </div>
                <div class="movie-detail-item">
                    <strong>Guionista</strong>
                    ${escapeHtml(movie.writer)}
                </div>
                <div class="movie-detail-item">
                    <strong>Actores</strong>
                    ${escapeHtml(movie.actors)}
                </div>
                <div class="movie-detail-item">
                    <strong>Idioma</strong>
                    ${escapeHtml(movie.language)}
                </div>
                <div class="movie-detail-item">
                    <strong>País</strong>
                    ${escapeHtml(movie.country)}
                </div>
                <div class="movie-detail-item">
                    <strong>Premios</strong>
                    ${escapeHtml(movie.awards || 'N/A')}
                </div>
                ${movie.boxOffice ? `
                <div class="movie-detail-item">
                    <strong>Recaudación</strong>
                    ${escapeHtml(movie.boxOffice)}
                </div>
                ` : ''}
            </div>
        </div>
    `;

    showSection(detailsSection);
}

function handleCloseDetails() {
    hideSection(detailsSection);
    showSection(resultsSection);
}

// ========================================
// FUNCIONES DE MENSAJES
// ========================================
function showMessage(text, type = 'success') {
    const messageDiv = document.createElement('div');
    messageDiv.className = `message message-${type}`;
    messageDiv.textContent = text;
    
    messageContainer.appendChild(messageDiv);
    
    setTimeout(() => {
        messageDiv.remove();
    }, 5000);
}

function clearMessages() {
    messageContainer.innerHTML = '';
}

// ========================================
// FUNCIONES DE UI
// ========================================
function showSection(section) {
    section.style.display = 'block';
}

function hideSection(section) {
    section.style.display = 'none';
}

function setButtonLoading(button, isLoading) {
    const btnText = button.querySelector('.btn-text');
    const btnLoader = button.querySelector('.btn-loader');
    
    if (isLoading) {
        btnText.style.display = 'none';
        btnLoader.style.display = 'inline';
        button.disabled = true;
    } else {
        btnText.style.display = 'inline';
        btnLoader.style.display = 'none';
        button.disabled = false;
    }
}

// ========================================
// UTILIDADES
// ========================================
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text || '';
    return div.innerHTML;
}

function formatVotes(votes) {
    if (!votes) return '0';
    // Eliminar comas del string (ej: "1,234,567" -> "1234567")
    const numVotes = parseInt(votes.replace(/,/g, ''));
    if (numVotes >= 1000000) {
        return (numVotes / 1000000).toFixed(1) + 'M';
    } else if (numVotes >= 1000) {
        return (numVotes / 1000).toFixed(1) + 'K';
    }
    return numVotes.toString();
}

// ========================================
// INICIALIZACIÓN
// ========================================
console.log('🎬 OMDB Movie Search inicializado');
console.log('API Base URL:', API_BASE_URL);
