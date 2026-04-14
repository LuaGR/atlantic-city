export const theme = {
  colors: {
    primary: '#0f1827',
    primaryContainer: '#242d3c',
    tertiaryContainer: '#cca71b',
    onTertiaryContainer: '#4e3e00',
    surface: '#f8f9fa',
    surfaceContainerLow: '#f3f4f5',
    surfaceContainerLowest: '#ffffff',
    surfaceContainerHigh: '#e7e8e9',
    secondary: '#505e7d',
    onSurfaceVariant: '#45474c',
    error: '#ba1a1a',
  },
  typography: {
    display: "'Manrope', sans-serif",
    body: "'Inter', sans-serif",
  },
  shadows: {
    cloud: '0 12px 40px rgba(15, 24, 39, 0.06)',
  },
  borders: {
    ghost: '1px solid rgba(197, 198, 204, 0.15)',
    focus: '1px solid rgba(15, 24, 39, 0.20)',
  }
};

export type ThemeType = typeof theme;
