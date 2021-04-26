const colors = require('tailwindcss/colors');

const production = !process.env.ROLLUP_WATCH;

module.exports = {
  future: {
    removeDeprecatedGapUtilities: true,
    purgeLayersByDefault: true,
  },
  plugins: [
    require("@tailwindcss/forms"),
  ],
  purge: {
    content: ["./src/**/*.svelte"],
    enabled: production,
  },
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {
      fontFamily: {
        body: ["Inter", "sans-serif"],
        logo: ["Comfortaa", "cursive"]
      },
      colors: {
        gray: colors.blueGray
      },
    },
  }
};