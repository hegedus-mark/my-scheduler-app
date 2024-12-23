  /** @type {import('tailwindcss').Config} */

  import daisyui from "daisyui"

 export default {
  content: [
    "./src/**/*.{html,ts}"
  ],
  theme: {
    extend: {
    },
  },
  plugins: [daisyui],
  daisyui: {
    themes:true,
  },
  important: true
}

