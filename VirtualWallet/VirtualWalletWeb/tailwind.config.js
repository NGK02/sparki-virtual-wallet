/** @type {import('tailwindcss').Config} */

module.exports = {
    darkMode: 'class',
    content: ["./views/**/*.cshtml"],
    theme: {
        extend: {
            animation: {
                'spin-slow': 'spin 5s linear infinite',
            },
            fontFamily: {
                'montserrat': ['Montserrat', 'sans-serif'],
            },
        },
  },
  plugins: [],
}