/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        pokeRed: {
          light: '#ff7675',
          DEFAULT: '#EF5350',
          dark: '#c0392b',
        },
        pokeBlack: {
          light: '#2d3436',
          DEFAULT: '#212121',
          dark: '#1e272e',
        },
        pokeYellow: {
          DEFAULT: '#FFCB05',
          dark: '#c39b00',
        },
        pokeBlue: {
          DEFAULT: '#3B4CCA',
        }
      },
      fontFamily: {
        sans: ['Outfit', 'Inter', 'sans-serif'],
      },
      boxShadow: {
        'poke-glow': '0 0 15px rgba(239, 83, 80, 0.6)',
        'yellow-glow': '0 0 15px rgba(255, 203, 5, 0.6)',
        'dark-glow': '0 0 15px rgba(33, 33, 33, 0.4)',
      }
    },
  },
  plugins: [],
}
