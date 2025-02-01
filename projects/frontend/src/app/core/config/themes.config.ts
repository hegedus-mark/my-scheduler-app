export interface ThemesConfig {
  id: string;
  name: string;
  icon?: string;
}

// Helper function to capitalize first letter
function capitalize(str: string): string {
  return str.charAt(0).toUpperCase() + str.slice(1);
}

// Create array of theme IDs
const themeIds = [
  "light",
  "dark",
  "cupcake",
  "bumblebee",
  "emerald",
  "corporate",
  "synthwave",
  "retro",
  "cyberpunk",
  "valentine",
  "halloween",
  "garden",
  "forest",
  "aqua",
  "lofi",
  "pastel",
  "fantasy",
  "wireframe",
  "black",
  "luxury",
  "dracula",
  "cmyk",
  "autumn",
  "business",
  "acid",
  "lemonade",
  "night",
  "coffee",
  "winter",
  "dim",
  "nord",
  "sunset",
] as const;

// Transform into ThemesConfig array
export const THEMES_CONFIG: ThemesConfig[] = themeIds.map((id) => ({
  id,
  name: id === "cmyk" ? "CMYK" : capitalize(id), // Special case for CMYK
}));
