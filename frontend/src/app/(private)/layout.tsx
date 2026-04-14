'use client';

import styled from 'styled-components';
import { useAuth } from '@/modules/auth/presentation/context/AuthContext';
import { useEffect } from 'react';
import { useRouter, usePathname } from 'next/navigation';

export default function PrivateLayout({ children }: { children: React.ReactNode }) {
  const { isAuthenticated } = useAuth();
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    if (!isAuthenticated) {
      router.push('/');
    }
  }, [isAuthenticated, router]);

  if (!isAuthenticated) return null;

  return (
    <LayoutContainer>
      <Navbar>
        <NavLeft>
          <LogoText>Atlantic City</LogoText>
        </NavLeft>
        <NavRight>
          <ProfileIcon>
            <span className="material-symbols-outlined">person</span>
          </ProfileIcon>
        </NavRight>
      </Navbar>

      <Sidebar>
        <SidebarHeader>
          <SidebarTitle>Gestor de Pedidos</SidebarTitle>
        </SidebarHeader>

        <NavMenu>
          <NavItem $active={pathname.includes('/pedidos')} onClick={() => router.push('/pedidos')}>
            <span className="material-symbols-outlined">shopping_cart</span>
            Pedidos
          </NavItem>
        </NavMenu>

        <LogoutBox>
          <LogoutBtn onClick={() => {
            localStorage.removeItem('token');
            router.push('/');
            setTimeout(() => window.location.reload(), 100);
          }}>
            <span className="material-symbols-outlined">logout</span>
            Cerrar Sesión
          </LogoutBtn>
        </LogoutBox>
      </Sidebar>

      <MainContent>{children}</MainContent>
    </LayoutContainer>
  );
}

const LayoutContainer = styled.div`
  min-height: 100vh;
  background-color: ${({ theme }) => theme.colors.surface};
  color: ${({ theme }) => theme.colors.primary};
`;

const Navbar = styled.nav`
  position: fixed;
  top: 0;
  width: 100%;
  z-index: 50;
  background: rgba(255, 255, 255, 0.8);
  backdrop-filter: blur(20px);
  border-bottom: 1px solid rgba(225, 227, 228, 0.5);
  box-shadow: 0 4px 12px rgba(15, 24, 39, 0.05);
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 2rem;
  height: 4rem;
`;

const NavLeft = styled.div`
  display: flex;
  align-items: center;
`;

const LogoText = styled.span`
  font-size: 1.25rem;
  font-weight: 800;
  letter-spacing: -0.05em;
  font-family: ${({ theme }) => theme.typography.display};
`;

const NavRight = styled.div`
  display: flex;
  align-items: center;
`;

const ProfileIcon = styled.div`
  width: 2rem;
  height: 2rem;
  border-radius: 50%;
  background-color: ${({ theme }) => theme.colors.surfaceContainer};
  display: flex;
  align-items: center;
  justify-content: center;
  color: ${({ theme }) => theme.colors.secondary};
  margin-left: 0.5rem;
  
  span {
    font-size: 18px;
  }
`;

const Sidebar = styled.aside`
  position: fixed;
  left: 0;
  top: 0;
  height: 100%;
  width: 16rem;
  padding-top: 5rem;
  background-color: ${({ theme }) => theme.colors.surfaceContainerLow};
  display: flex;
  flex-direction: column;
  padding-bottom: 1rem;
`;

const SidebarHeader = styled.div`
  padding: 0 1.5rem;
  margin-bottom: 2rem;
`;

const SidebarTitle = styled.h2`
  font-size: 1.125rem;
  font-weight: 900;
`;

const NavMenu = styled.div`
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
`;

const NavItem = styled.a<{ $active?: boolean }>`
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.75rem 1.5rem;
  font-size: 0.875rem;
  font-family: ${({ theme }) => theme.typography.display};
  font-weight: ${({ $active }) => ($active ? '700' : '500')};
  color: ${({ theme, $active }) => ($active ? theme.colors.primary : theme.colors.secondary)};
  background-color: ${({ $active }) => ($active ? '#fff' : 'transparent')};
  border-left: ${({ theme, $active }) => ($active ? `4px solid ${theme.colors.tertiaryContainer}` : '4px solid transparent')};
  cursor: pointer;
  transition: all 0.2s;

  &:hover {
    background-color: ${({ $active }) => ($active ? '#fff' : 'rgba(225, 227, 228, 0.5)')};
  }
`;

const LogoutBox = styled.div`
  margin-top: auto;
  border-top: 1px solid rgba(225, 227, 228, 0.5);
  padding-top: 1rem;
`;

const LogoutBtn = styled.button`
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.75rem 1.5rem;
  font-size: 0.875rem;
  font-family: ${({ theme }) => theme.typography.display};
  font-weight: 500;
  color: ${({ theme }) => theme.colors.secondary};
  background: none;
  border: none;
  width: 100%;
  cursor: pointer;
  text-align: left;
  border-left: 4px solid transparent;

  &:hover {
    background-color: rgba(225, 227, 228, 0.5);
  }
`;

const MainContent = styled.main`
  margin-left: 16rem;
  padding-top: 4rem;
  min-height: 100vh;
`;
