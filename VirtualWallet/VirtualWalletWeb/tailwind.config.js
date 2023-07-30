/** @type {import('tailwindcss').Config} */

module.exports = {
    darkMode: 'class',
    content: ["./views/**/*.cshtml"],
    theme: {
        extend: {
            fontFamily: {
                'montserrat': ['Montserrat', 'sans-serif'],
            },
        },
  },
  plugins: [],
}