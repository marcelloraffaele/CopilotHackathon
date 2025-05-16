// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Memory Game Logic
// Star Wars characters map
const starWarsCharacters = [
    { id: 1, name: "Luke Skywalker", image: "https://vignette.wikia.nocookie.net/starwars/images/2/20/LukeTLJ.jpg" },
    { id: 2, name: "C-3PO", image: "https://vignette.wikia.nocookie.net/starwars/images/3/3f/C-3PO_TLJ_Card_Trader_Award_Card.png" },
    { id: 3, name: "R2-D2", image: "https://vignette.wikia.nocookie.net/starwars/images/e/eb/ArtooTFA2-Fathead.png" },
    { id: 4, name: "Darth Vader", image: "https://vignette.wikia.nocookie.net/fr.starwars/images/3/32/Dark_Vador.jpg" },
    { id: 5, name: "Leia Organa", image: "https://vignette.wikia.nocookie.net/starwars/images/f/fc/Leia_Organa_TLJ.png" },
    { id: 6, name: "Owen Lars", image: "https://vignette.wikia.nocookie.net/starwars/images/e/eb/OwenCardTrader.png" },
    { id: 7, name: "Beru Whitesun lars", image: "https://vignette.wikia.nocookie.net/starwars/images/c/cc/BeruCardTrader.png" },
    { id: 8, name: "R5-D4", image: "https://vignette.wikia.nocookie.net/starwars/images/c/cb/R5-D4_Sideshow.png" },
    { id: 9, name: "Biggs Darklighter", image: "https://vignette.wikia.nocookie.net/starwars/images/0/00/BiggsHS-ANH.png" },
    { id: 10, name: "Obi-Wan Kenobi", image: "https://vignette.wikia.nocookie.net/starwars/images/4/4e/ObiWanHS-SWE.jpg" },
    { id: 11, name: "Anakin Skywalker", image: "https://vignette.wikia.nocookie.net/starwars/images/6/6f/Anakin_Skywalker_RotS.png" },
    { id: 12, name: "Wilhuff Tarkin", image: "https://vignette.wikia.nocookie.net/starwars/images/c/c1/Tarkininfobox.jpg" },
    { id: 13, name: "Chewbacca", image: "https://vignette.wikia.nocookie.net/starwars/images/4/48/Chewbacca_TLJ.png" },
    { id: 14, name: "Han Solo", image: "https://vignette.wikia.nocookie.net/starwars/images/e/e2/TFAHanSolo.png" },
    { id: 15, name: "Greedo", image: "https://vignette.wikia.nocookie.net/starwars/images/c/c6/Greedo.jpg" },
    { id: 16, name: "Jabba Desilijic Tiure", image: "https://vignette.wikia.nocookie.net/starwars/images/7/7f/Jabba_SWSB.png" },
    { id: 17, name: "Wedge Antilles", image: "https://vignette.wikia.nocookie.net/starwars/images/6/60/WedgeHelmetless-ROTJHD.jpg" },
    { id: 18, name: "Jek Tono Porkins", image: "https://vignette.wikia.nocookie.net/starwars/images/e/eb/JekPorkins-DB.png" },
    { id: 19, name: "Yoda", image: "https://vignette.wikia.nocookie.net/starwars/images/d/d6/Yoda_SWSB.png" },
    { id: 20, name: "Palpatine", image: "https://vignette.wikia.nocookie.net/starwars/images/d/d8/Emperor_Sidious.png" },
    { id: 21, name: "Boba Fett", image: "https://vignette.wikia.nocookie.net/starwars/images/7/79/Boba_Fett_HS_Fathead.png" },
    { id: 22, name: "IG-88", image: "https://vignette.wikia.nocookie.net/starwars/images/f/f2/IG-88.png" },
    { id: 23, name: "Bossk", image: "https://vignette.wikia.nocookie.net/starwars/images/1/1d/Bossk.png" },
    { id: 24, name: "Lando Calrissian", image: "https://vignette.wikia.nocookie.net/starwars/images/8/8f/Lando_ROTJ.png" },
    { id: 25, name: "Lobot", image: "https://vignette.wikia.nocookie.net/starwars/images/9/96/SWE_Lobot.jpg" },
    { id: 26, name: "Ackbar", image: "https://vignette.wikia.nocookie.net/starwars/images/2/29/Admiral_Ackbar_RH.png" },
    { id: 27, name: "Mon Mothma", image: "https://vignette.wikia.nocookie.net/starwars/images/b/b7/MP-MonMothma.png" },
    { id: 28, name: "Arvel Crynyd", image: "https://vignette.wikia.nocookie.net/starwars/images/d/de/Arvel-crynyd.jpg" },
    { id: 29, name: "Wicket Systri Warrick", image: "https://vignette.wikia.nocookie.net/starwars/images/4/4f/Wicket_RotJ.png" },
    { id: 30, name: "Nien Nunb", image: "https://vignette.wikia.nocookie.net/starwars/images/1/14/Old_nien_nunb_-_profile.png" },
    { id: 31, name: "Qui-Gon Jinn", image: "https://vignette.wikia.nocookie.net/starwars/images/f/f6/Qui-Gon_Jinn_Headshot_TPM.jpg" }
];

if (typeof gridSize === 'undefined') {
    var gridSize = 4;
    var playerCount = 1;
    var cards = [];
    var flippedCards = [];
    var matchedCards = [];
    var currentPlayer = 1;
    var scores = [0, 0];
    var lockBoard = false;
}

function shuffle(array) {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
}

function createCards(size) {
    const numPairs = (size * size) / 2;
    // Use Star Wars characters instead of colorEmojiPairs
    let pairs = starWarsCharacters.slice(0, numPairs);
    let symbols = [];
    pairs.forEach(pair => {
        symbols.push({ ...pair });
        symbols.push({ ...pair });
    });
    return shuffle(symbols);
}

function renderGrid() {
    const grid = document.getElementById('gameGrid');
    grid.innerHTML = '';
    grid.style.gridTemplateColumns = `repeat(${gridSize}, 100px)`;
    grid.style.gridTemplateRows = `repeat(${gridSize}, 100px)`;
    grid.className = 'memory-grid';
    cards.forEach((symbol, idx) => {
        const card = document.createElement('div');
        card.className = 'memory-card';
        card.dataset.index = idx;
        card.innerHTML = '<span class="symbol"></span>';
        card.addEventListener('click', onCardClick);
        grid.appendChild(card);
    });
}

function onCardClick(e) {
    if (lockBoard) return;
    const cardDiv = e.currentTarget;
    const idx = parseInt(cardDiv.dataset.index);
    if (flippedCards.includes(idx) || matchedCards.includes(idx)) return;
    flipCard(cardDiv, idx);
    flippedCards.push(idx);
    if (flippedCards.length === 2) {
        lockBoard = true;
        setTimeout(checkMatch, 800);
    }
}

function flipCard(cardDiv, idx) {
    cardDiv.classList.add('flipped');
    const symbol = cards[idx];
    cardDiv.style.background = `url('${symbol.image}') center center / cover no-repeat`;
    cardDiv.style.border = '2px solid #ffd700';
    cardDiv.style.boxShadow = '0 8px 16px rgba(0,0,0,0.3)';
}

function unflipCard(idx) {
    const grid = document.getElementById('gameGrid');
    const cardDiv = grid.children[idx];
    cardDiv.classList.remove('flipped');
    cardDiv.style.background = '#222';
    cardDiv.style.border = '2px solid #333';
    cardDiv.style.boxShadow = '0 4px 8px rgba(0,0,0,0.2)';
}

function checkMatch() {
    const [idx1, idx2] = flippedCards;
    // Compare by emoji and color, not by object reference
    if (
        cards[idx1].name === cards[idx2].name &&
        cards[idx1].image === cards[idx2].image
    ) {
        matchedCards.push(idx1, idx2);
        scores[currentPlayer - 1]++;
        if (matchedCards.length === cards.length) {
            showStatus(`Player ${currentPlayer} wins!`);
            lockBoard = true;
            return;
        }
    } else {
        setTimeout(() => {
            unflipCard(idx1);
            unflipCard(idx2);
        }, 400);
        if (playerCount === 2) {
            currentPlayer = currentPlayer === 1 ? 2 : 1;
        }
    }
    flippedCards = [];
    lockBoard = false;
    updateStatus();
}

function showStatus(msg) {
    document.getElementById('gameStatus').textContent = msg;
}

function updateStatus() {
    if (playerCount === 1) {
        showStatus(`Score: ${scores[0]}`);
    } else {
        showStatus(`Player 1: ${scores[0]} | Player 2: ${scores[1]} | Turn: Player ${currentPlayer}`);
    }
}

function startGame() {
    gridSize = parseInt(document.getElementById('gridSize').value);
    playerCount = parseInt(document.getElementById('playerCount').value);
    cards = createCards(gridSize);
    flippedCards = [];
    matchedCards = [];
    currentPlayer = 1;
    scores = [0, 0];
    lockBoard = false;
    renderGrid();
    updateStatus();
}

document.getElementById('startGameBtn').addEventListener('click', startGame);

// Initial render
document.addEventListener('DOMContentLoaded', () => {
    startGame();
});
