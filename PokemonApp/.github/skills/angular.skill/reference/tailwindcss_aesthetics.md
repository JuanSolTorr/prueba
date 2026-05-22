# Skill: Tailwind CSS & Modern Web Aesthetics

This document outlines design systems, theme parameters, animations, and aesthetic standards when styling the Pokémon Application using Tailwind CSS 3.

---

## 1. Design System Colors & Variables

- **Brand Palettes:**
  - Avoid using standard saturated Tailwind primary colors (like default `bg-red-500` or `bg-blue-500`).
  - Rely exclusively on the custom extending color scheme in `tailwind.config.js`:
    - **PokeRed:** `#EF5350` (Default), `#ff7675` (Light), `#c0392b` (Dark)
    - **PokeBlack:** `#212121` (Default), `#2d3436` (Light), `#1e272e` (Dark)
    - **PokeYellow:** `#FFCB05` (Default), `#c39b00` (Dark)
    - **PokeBlue:** `#3B4CCA` (Default)
- **Typography:**
  - The default font family is `Outfit` (fallback to `Inter`). Ensure headers and labels utilize proper letter-spacing (`tracking-wide` or `tracking-wider`) and varying font-weights (`font-semibold` / `font-bold`) to create strong visual hierarchies.

---

## 2. Glassmorphism & High-End Cards

To present a premium, sleek feel, UI cards and overlay components must use translucent glass styles rather than flat block backgrounds:

- **Glass Panels:**
  - Apply backdrop blurs combined with translucent backgrounds and subtle, thin borders:
    ```css
    .glass-card {
      background: rgba(33, 33, 33, 0.6);
      backdrop-filter: blur(8px);
      border: 1px solid rgba(255, 255, 255, 0.05);
    }
    ```
  - This allows background gradients to bleed through, giving the layout depth.
- **Card Interactive States:**
  - Hover states should slightly lift the card and highlight the borders:
    ```css
    .glass-card:hover {
      border-color: rgba(239, 83, 80, 0.3);
      transform: translateY(-4px);
    }
    ```

---

## 3. Micro-Animations & Dynamic Interactions

An interface that feels responsive and alive encourages interaction. Incorporate micro-animations at interactive points:

- **Hover Shake (`hover-shake`):**
  - Use keyframes to add quick, minor rotations to buttons, badges, or item graphics on hover to simulate responsiveness.
- **Glow Effects:**
  - Enhance active state indicators (like selected types or item categories) with pulsing shadows:
    - **PokeRed Glow (`animate-pulse-glow`):** `box-shadow: 0 0 20px rgba(239, 83, 80, 0.7)`
    - **PokeYellow Glow (`animate-pulse-yellow`):** `box-shadow: 0 0 20px rgba(255, 203, 5, 0.7)`
- **Floats and Scale entries:**
  - Elements in focus or primary artwork should slowly float (`animate-float`) or expand softly (`animate-beam`) to feel alive.

---

## 4. Layout & Grid Rules

- **Smooth Grid Responsive Wrapping:**
  - Ensure catalogs wrap cleanly on varying screen widths:
    - `grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-6`
- **Overflow & Scrollbars:**
  - Style default scrollbars to blend in with the dark theme. Use thin, customized scrollbar colors (`bg-pokeRed` on hover) to match the visual language.
