# Project creation
```bash
dotnet new razor -n MemoryGameWeb
```


# Prompts
## Game generation
```bash
Work in the directory MemoryGameWeb. When run terminal remember to change the right directory, for example: `cd MemoryGameWeb && dotnet test`.
This Web application implements a memory game. The game consists of a grid of cards. Each card has a symbol on one side. The cards are arranged randomly on the grid with the symbol face down. The player flips two cards over each turn. If the two cards have the same symbol, they remain face up. Otherwise, the cards flip back over after a short period of time. The goal of the game is to match all pairs of cards.

The grid should be 4x4, 6x6, or 8x8. Use the numbers from 1 to 32 as symbols. 

The mechanic of the game is as follows:
- Create a grid of cards. 
- Choose number of players. The game can be played by one or two players.
- Add a card to the grid. The card should have a symbol on one side.
- Shuffle the cards on the grid.
    - Add a click event to each card. When the player clicks a card, the card flips over.
    - When the player clicks two cards, check if the cards have the same symbol. If the cards have the same symbol, the cards remain face up. Otherwise, the cards flip back over.
    - When all pairs of cards are matched, the player wins the game.
```


## Use Emojis
```
change the theme of the game to a different theme. Use colors and emojis instead of numbers. 
```

## Use Star Wars Images

### Generate Star Wars Images file
```

create a powershell script named `generateStarWarsImagefile.ps1` that will generate a file `starwars-images.json` calling 32 times the api: https://akabab.github.io/starwars-api/api/id/<ID>.json with ID=1 to 32. 
the json file contains the following fields:
```
{
  "id": 2,
  "name": "C-3PO",
  "height": 1.71,
  "mass": 75,
  "gender": "male",
  "homeworld": "tatooine",
  "species": "droid",
  "wiki": "http://starwars.wikia.com/wiki/C-3PO",
  "image": "https://vignette.wikia.nocookie.net/starwars/images/3/3f/C-3PO_TLJ_Card_Trader_Award_Card.png",
  "dateCreated": -32,
  "dateDestroyed": 3,
  "destroyedLocation": "bespin, rebuilt shortly after",
  "creator": "Anakin Skywalker",
  "manufacturer": "Cybot Galactica",
  "model": "3PO unit",
  "class": "Protocol droid",
  "sensorColor": "yellow",
  "platingColor": "gray, later primarily golden",
  "equipment": "TranLang III communication module",
  "affiliations": [
    "Skywalker family",
    "Confederacy of Independent Systems",
    "Royal House of Naboo",
    "Galactic Republic",
    "House of Organa",
    "Galactic Empire",
    "Alliance to Restore the Republic",
    "Massassi Group",
    "Leia Organa's team",
    "Pathfinders",
    "Endor strike team",
    "Bright Tree tribe",
    "New Republic",
    "Resistance",
    "Resistance spy droid network"
  ],
  "skinColor": "gold",
  "eyeColor": "yellow",
  "born": -112,
  "formerAffiliations": []
}
```
the json resulting file should contain the following fields:
- id
- name
- image
```

### Change the game cards using the images
```
change the game cards using the images from the file `starwars-images.json`.
Set the image as background of the card and be sure that the image is centered and cover the entire card.
The card should have a border and a shadow. The card should be flipped when the player clicks on it. The card should be flipped back when the player clicks on another card. The card should be flipped back after a short period of time if the cards do not match.
```

# run the application
```bash
cd MemoryGameWeb
dotnet run
``` 