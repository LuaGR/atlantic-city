'use client';

import { useState } from 'react';
import styled from 'styled-components';
import { useAuth } from '@/modules/auth/presentation/context/AuthContext';
import { AuthService } from '@/modules/auth/infrastructure/auth.service';
import { AxiosError } from 'axios';

export default function LoginPage() {
  const { login } = useAuth();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      const response = await AuthService.login(email, password);
      login(response.token);
    } catch (err: unknown) {
      if (err instanceof AxiosError) {
        if (err.response?.status === 401 || err.response?.status === 400) {
          setError('Credenciales inválidas. Intente nuevamente.');
        } else {
          setError('Error al conectarse con el servidor.');
        }
      } else {
        setError('Ocurrió un error inesperado.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container>
      <BgDecoration />
      <DropIcon>
        <span className="material-symbols-outlined">water_drop</span>
      </DropIcon>

      <LoginWrapper>
        <Header>
          <LogoBox>
            <span className="material-symbols-outlined">account_balance</span>
          </LogoBox>
          <Title>Atlantic City</Title>
          <Subtitle>ADMINISTRACIÓN EMPRESARIAL</Subtitle>
        </Header>

        <Card>
          <CardHeader>
            <CardTitle>Acceso al Portal</CardTitle>
            <CardDesc>Ingrese sus credenciales corporativas.</CardDesc>
          </CardHeader>

          <Form onSubmit={handleLogin}>
            {error && <ErrorMessage>{error}</ErrorMessage>}

            <FormGroup>
              <Label htmlFor="email">Correo Corporativo</Label>
              <InputContainer>
                <Icon className="material-symbols-outlined">mail</Icon>
                <Input
                  type="email"
                  id="email"
                  placeholder="nombre@atlanticcity.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                />
              </InputContainer>
            </FormGroup>

            <FormGroup>
              <LabelRow>
                <Label htmlFor="password">Contraseña</Label>
              </LabelRow>
              <InputContainer>
                <Icon className="material-symbols-outlined">lock</Icon>
                <Input
                  type="password"
                  id="password"
                  placeholder="••••••••"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                />
              </InputContainer>
            </FormGroup>

            <SubmitButton type="submit" disabled={loading}>
              {loading ? 'Accediendo...' : 'Acceder al Sistema'}
              {!loading && <span className="material-symbols-outlined">arrow_forward</span>}
            </SubmitButton>
          </Form>

          <FooterArea>
            <span className="material-symbols-outlined" style={{ fontSize: '18px' }}>shield_lock</span>
            <FooterText>
              Solo personal autorizado. El acceso es monitoreado acorde a la política de seguridad.
            </FooterText>
          </FooterArea>
        </Card>
      </LoginWrapper>
    </Container>
  );
}

const Container = styled.div`
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 1.5rem;
  position: relative;
  overflow: hidden;
`;

const BgDecoration = styled.div`
  position: absolute;
  top: 0;
  left: 0;
  width: 33.33%;
  height: 100%;
  background-color: ${({ theme }) => theme.colors.surfaceContainerLow};
  z-index: -1;
  opacity: 0.5;
`;

const DropIcon = styled.div`
  position: absolute;
  bottom: 3rem;
  right: 3rem;
  z-index: -1;
  color: ${({ theme }) => theme.colors.surfaceContainerHigh};
  user-select: none;
  pointer-events: none;
  
  span {
    font-size: 200px;
    font-variation-settings: 'FILL' 0, 'wght' 400, 'GRAD' 0, 'opsz' 24;
  }
`;

const LoginWrapper = styled.div`
  width: 100%;
  max-width: 440px;
  display: flex;
  flex-direction: column;
  align-items: center;
`;

const Header = styled.div`
  margin-bottom: 3rem;
  text-align: center;
`;

const LogoBox = styled.div`
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 48px;
  height: 48px;
  background-color: ${({ theme }) => theme.colors.primary};
  border-radius: 12px;
  margin-bottom: 1.5rem;
  box-shadow: ${({ theme }) => theme.shadows.cloud};

  span {
    color: ${({ theme }) => theme.colors.tertiaryContainer};
    font-size: 24px;
    font-variation-settings: 'FILL' 0, 'wght' 400, 'GRAD' 0, 'opsz' 24;
  }
`;

const Title = styled.h1`
  font-size: 1.5rem;
  font-weight: 800;
  letter-spacing: -0.05em;
  color: ${({ theme }) => theme.colors.primary};
  text-transform: uppercase;
`;

const Subtitle = styled.p`
  color: ${({ theme }) => theme.colors.secondary};
  font-size: 0.875rem;
  font-weight: 500;
  margin-top: 0.25rem;
  letter-spacing: 0.025em;
`;

const Card = styled.div`
  width: 100%;
  background-color: ${({ theme }) => theme.colors.surfaceContainerLowest};
  border-radius: 12px;
  padding: 2.5rem;
  box-shadow: ${({ theme }) => theme.shadows.cloud};
`;

const CardHeader = styled.header`
  margin-bottom: 2rem;
`;

const CardTitle = styled.h2`
  font-size: 1.25rem;
  font-weight: 700;
  color: ${({ theme }) => theme.colors.primary};
  letter-spacing: -0.025em;
`;

const CardDesc = styled.p`
  color: ${({ theme }) => theme.colors.onSurfaceVariant};
  font-size: 0.875rem;
  margin-top: 0.25rem;
`;

const ErrorMessage = styled.div`
  background-color: ${({ theme }) => theme.colors.error}15; /* 15% opacity */
  color: ${({ theme }) => theme.colors.error};
  padding: 0.875rem;
  border-radius: 8px;
  font-size: 0.875rem;
  font-weight: 500;
  text-align: center;
`;

const Form = styled.form`
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
`;

const FormGroup = styled.div`
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
`;

const LabelRow = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

const Label = styled.label`
  font-size: 0.75rem;
  font-weight: 700;
  color: ${({ theme }) => theme.colors.secondary};
  text-transform: uppercase;
  letter-spacing: 0.05em;
`;

const InputContainer = styled.div`
  position: relative;
`;

const Icon = styled.span`
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: ${({ theme }) => theme.colors.onSurfaceVariant};
  font-size: 18px;
  font-variation-settings: 'FILL' 0, 'wght' 400, 'GRAD' 0, 'opsz' 24;
`;

const Input = styled.input`
  width: 100%;
  padding: 0.75rem 1rem 0.75rem 3rem;
  background-color: ${({ theme }) => theme.colors.surfaceContainerHigh};
  border: none;
  border-radius: 8px;
  font-size: 0.875rem;
  color: ${({ theme }) => theme.colors.primary};
  outline: none;
  transition: all 0.2s;

  &::placeholder {
    color: ${({ theme }) => theme.colors.onSurfaceVariant};
  }

  &:focus {
    background-color: ${({ theme }) => theme.colors.surfaceContainerLowest};
    border: ${({ theme }) => theme.borders.focus};
    padding: 0.7rem 0.95rem 0.7rem 2.95rem; /* Compensar el 1px del borde para que no salte, o usar boxShadow */
    box-shadow: inset 0 0 0 1px rgba(15, 24, 39, 0.20);
    border: none;
    padding: 0.75rem 1rem 0.75rem 3rem;
  }
`;

const SubmitButton = styled.button`
  width: 100%;
  background-color: ${({ theme }) => theme.colors.tertiaryContainer};
  color: ${({ theme }) => theme.colors.onTertiaryContainer};
  font-weight: 700;
  padding: 1rem;
  border-radius: 8px;
  border: none;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  cursor: pointer;
  transition: all 0.2s;
  margin-top: 0.5rem;

  font-size: 0.875rem;
  letter-spacing: -0.025em;

  span {
    font-size: 20px;
    font-variation-settings: 'FILL' 0, 'wght' 400, 'GRAD' 0, 'opsz' 24;
  }

  &:hover {
    opacity: 0.9;
  }

  &:active {
    transform: scale(0.98);
  }

  &:disabled {
    opacity: 0.6;
    cursor: not-allowed;
  }
`;

const FooterArea = styled.footer`
  margin-top: 2.5rem;
  padding-top: 2rem;
  border-top: 1px solid rgba(225, 227, 228, 0.5); /* surface-variant */
  display: flex;
  align-items: flex-start;
  gap: 1rem;
  color: ${({ theme }) => theme.colors.onSurfaceVariant};
`;

const FooterText = styled.p`
  font-size: 0.6875rem; /* 11px */
  line-height: 1.5;
`;
