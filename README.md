# Space Pachinko
 
A space themed Pachinko game built in Unity for Assignment 1 of VGP236 Programming for Game Engines at LaSalle College Vancouver.
 
Drop balls from a ship at the top of the board, watch them bounce through pegs and obstacles, and land them in scoring slots at the bottom. Harder slots are worth more points.
 
## Status:
<span style="font-size: 1.5em; font-weight: bold; background-color: none;">In Development.</span>

---

| Phase | Topic | Status |
|---|---|---|
| 0 to 3 | Pre-production and project setup | Done |
| 4 | Board and core physics | Done |
| 5 | Spawner | Next |
| 6 to 13 | Obstacles, scoring, game loop, menus, audio, polish, testing | Not started |
 
## Requirements
 
- Unity 6.3 LTS (6000.3.26f1)
- Universal Render Pipeline (2D Renderer), included in the project
## How to Open
 
1. Open the project folder in Unity Hub with Unity 6.3 LTS.
2. Open the scene `Assets/_Project/Scenes/Game.unity`.
3. Resolution: this game is built for 768x1024 portrait. Before pressing Play, open the Game view’s resolution dropdown and select 768x1024. If it isn’t listed, click +, choose Type: Fixed Resolution, and enter Width 768 and Height 1024. Free Aspect crops or stretches the board.

Start scene: open Assets/_Project/Scenes/MainMenu.unity and press Play.
4. Press Play.
## Controls
 
| Action | Key |
|---|---|
| Move spawner | Arrow keys or WASD |
| Drop ball | Spacebar |
| Pause | Planned |
 
Controls are planned for Phase 5 and later and may not work yet.
 
## Project Structure
 
| Folder | Contents |
|---|---|
| `Assets/_Project` | All original work: scenes, scripts, prefabs, physics materials |
| `Assets/ThirdParty` | Third party art, audio and fonts, each with its license file |
 
## Credits
 
Developed by Vishesh Mistry, BSc Game Programming, LaSalle College Vancouver.
Course: VGP236 Programming for Game Engines. Instructor: Darren Waine.
 
### Third Party Assets
 
| Asset | Author | License |
|---|---|---|
| Space Shooter Redux, Planets, UI Pack Sci-Fi, UI Pack, Particle Pack, Game Icons | Kenney (www.kenney.nl) | CC0 1.0 |
| Interface Sounds, Impact Sounds, Digital Audio, Sci-fi Sounds, Music Jingles | Kenney (www.kenney.nl) | CC0 1.0 |
| "Outer Space Loop" (menu music) | wipics (OpenGameArt.org) | CC0 |
| "Space Echo" (gameplay music) | Centurion_of_war (OpenGameArt.org) | CC0 |
| Orbitron font | The Orbitron Project Authors | SIL Open Font License 1.1 |