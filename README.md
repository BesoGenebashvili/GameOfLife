# Game of Life — Console Edition

![Demo](assets/demo.gif)

## Overview
This project renders and animates Conway's Game of Life entirely in the terminal.  
Built as a fun exploration of terminal UI and rendering in .NET.

## Technologies
- Spectre.Console for terminal UI  
- Terminalizer for demo recording

## Rules

The Game of Life is a cellular automaton invented by mathematician John Conway.  
Each cell is either **alive (1)** or **dead (0)**.

### For a live (populated) cell:
- Fewer than 2 neighbors → **dies** (as if by solitude)  
- More than 3 neighbors → **dies** (as if by overpopulation)  
- 2 or 3 neighbors → **survives**

### For a dead (empty) cell:
- Exactly 3 neighbors → **becomes alive** (reproduction)
