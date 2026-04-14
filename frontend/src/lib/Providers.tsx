'use client';

import { ThemeProvider } from 'styled-components';
import type { ReactNode } from 'react';
import { theme } from '@/styles/theme';
import StyledComponentsRegistry from '@/lib/registry';
import { GlobalStyles } from '@/styles/GlobalStyles';
import { AuthProvider } from '@/modules/auth/presentation/context/AuthContext';

export default function Providers({ children }: { children: ReactNode }) {
  return (
    <StyledComponentsRegistry>
      <ThemeProvider theme={theme}>
        <AuthProvider>
          <GlobalStyles />
          {children}
        </AuthProvider>
      </ThemeProvider>
    </StyledComponentsRegistry>
  );
}
