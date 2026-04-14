'use client';

import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { useRouter, useParams } from 'next/navigation';
import { PedidoService } from '@/modules/pedidos/infrastructure/pedido.service';
import { AxiosError } from 'axios';

export default function EditarPedidoPage() {
  const router = useRouter();
  const params = useParams();
  const id = Number(params.id);

  const [loading, setLoading] = useState(false);
  const [fetching, setFetching] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  const [form, setForm] = useState({
    numeroPedido: '',
    cliente: '',
    fecha: '',
    total: '',
    estado: 'Registrado',
  });

  useEffect(() => {
    if (!id) return;
    fetchPedido();
  }, [id]);

  const fetchPedido = async () => {
    try {
      const data = await PedidoService.getById(id);
      setForm({
        numeroPedido: data.numeroPedido,
        cliente: data.cliente,
        fecha: data.fecha.split('T')[0],
        total: data.total.toString(),
        estado: data.estado,
      });
    } catch (err: unknown) {
      if (err instanceof AxiosError && err.response?.status === 404) {
        setError('El pedido no existe.');
      } else {
        setError('No se pudo cargar la información del pedido.');
      }
    } finally {
      setFetching(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    
    try {
      await PedidoService.update(id, {
        cliente: form.cliente,
        fecha: new Date(form.fecha).toISOString(),
        total: parseFloat(form.total) || 0,
        estado: form.estado,
      });
      router.push('/pedidos'); 
    } catch (err: unknown) {
      if (err instanceof AxiosError && err.response?.data) {
        let backendMsg = 'Error procesando la actualización';
        if (typeof err.response.data === 'string') {
          backendMsg = err.response.data;
        } else if (err.response.data.title || err.response.data.detail) {
          backendMsg = err.response.data.detail || err.response.data.title;
        } else if (err.response.data.errors) {
          backendMsg = Object.values(err.response.data.errors).flat().join(', ');
        }
        setError(backendMsg);
      } else {
        setError('No se pudo conectar con el servidor.');
      }
    } finally {
      setLoading(false);
    }
  };

  if (fetching) {
    return (
      <PageContainer>
        <LoadingState>Cargando información del pedido...</LoadingState>
      </PageContainer>
    );
  }

  return (
    <PageContainer>
      <Header>
        <Breadcrumb>
          Pedidos <span className="material-symbols-outlined">chevron_right</span> 
          <strong>Editar Pedido</strong>
        </Breadcrumb>
        <Title>Editar Pedido #{form.numeroPedido}</Title>
      </Header>

      <Grid>
        <MainSection>
          <FormCard onSubmit={handleSubmit}>
            <SectionTitle>Modificar Información</SectionTitle>
            
            {error && <ErrorMessage>{error}</ErrorMessage>}

            <FormGrid>
              <FormGroup>
                <Label>Número de Referencia</Label>
                <Input 
                  type="text" 
                  value={form.numeroPedido}
                  disabled 
                />
              </FormGroup>

              <FormGroup>
                <Label>Cliente</Label>
                <Input 
                  type="text" 
                  placeholder="Nombre de la empresa o cliente" 
                  value={form.cliente}
                  onChange={(e) => setForm({...form, cliente: e.target.value})}
                  required 
                />
              </FormGroup>

              <FormGroup>
                <Label>Fecha del Pedido</Label>
                <Input 
                  type="date" 
                  value={form.fecha}
                  onChange={(e) => setForm({...form, fecha: e.target.value})}
                  required 
                />
              </FormGroup>

              <FormGroup>
                <Label>Monto Total ($)</Label>
                <Input 
                  type="number" 
                  step="0.01" 
                  placeholder="0.00" 
                  value={form.total}
                  onChange={(e) => setForm({...form, total: e.target.value})}
                  required 
                />
              </FormGroup>

              <FormGroup>
                <Label>Estado</Label>
                <Select 
                  value={form.estado}
                  onChange={(e) => setForm({...form, estado: e.target.value})}
                >
                  <option value="Registrado">Registrado</option>
                  <option value="Procesado">Procesado</option>
                  <option value="Enviado">Enviado</option>
                  <option value="Entregado">Entregado</option>
                  <option value="Cancelado">Cancelado</option>
                </Select>
              </FormGroup>
            </FormGrid>

            <Actions>
              <CancelBtn type="button" onClick={() => router.push('/pedidos')}>
                Cancelar / Volver
              </CancelBtn>
              <SubmitBtn type="submit" disabled={loading}>
                {loading ? 'Guardando...' : (
                  <>
                    <span className="material-symbols-outlined">save</span>
                    Guardar Cambios
                  </>
                )}
              </SubmitBtn>
            </Actions>
          </FormCard>
        </MainSection>
      </Grid>
    </PageContainer>
  );
}

const PageContainer = styled.div`
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
`;

const Header = styled.header`
  margin-bottom: 3rem;
`;

const Breadcrumb = styled.div`
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: ${({ theme }) => theme.colors.onSurfaceVariant};
  font-size: 0.875rem;
  margin-bottom: 1rem;

  strong {
    color: ${({ theme }) => theme.colors.primary};
    font-weight: 500;
  }

  span {
    font-size: 1rem;
  }
`;

const Title = styled.h1`
  font-size: 2.25rem;
  font-weight: 800;
  letter-spacing: -0.05em;
  color: ${({ theme }) => theme.colors.primary};
`;

const Grid = styled.div`
  display: flex;
  flex-direction: column;
  gap: 2rem;
  max-width: 800px;
`;

const MainSection = styled.section`
  display: flex;
  flex-direction: column;
  gap: 2rem;
`;

const FormCard = styled.form`
  background-color: ${({ theme }) => theme.colors.surfaceContainerLowest};
  padding: 2rem;
  border-radius: 0.75rem;
  box-shadow: ${({ theme }) => theme.shadows.cloud};
`;

const SectionTitle = styled.h3`
  font-size: 0.875rem;
  font-weight: 700;
  color: ${({ theme }) => theme.colors.primary};
  text-transform: uppercase;
  letter-spacing: 0.1em;
  margin-bottom: 1.5rem;
`;

const ErrorMessage = styled.div`
  background-color: ${({ theme }) => theme.colors.error}15;
  color: ${({ theme }) => theme.colors.error};
  padding: 0.875rem;
  border-radius: 8px;
  font-size: 0.875rem;
  font-weight: 500;
  margin-bottom: 1.5rem;
`;

const FormGrid = styled.div`
  display: grid;
  grid-template-columns: 1fr;
  gap: 1.5rem 2rem;

  @media (min-width: 768px) {
    grid-template-columns: 1fr 1fr;
  }
`;

const FormGroup = styled.div`
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
`;

const Label = styled.label`
  font-size: 0.75rem;
  font-weight: 600;
  color: ${({ theme }) => theme.colors.onSurfaceVariant};
`;

const inputStyles = `
  width: 100%;
  background-color: #e7e8e9;
  border: none;
  border-radius: 0.5rem;
  padding: 0.75rem 1rem;
  font-size: 0.875rem;
  color: #0f1827;
  outline: none;
  transition: all 0.2s;

  &:focus:not(:disabled) {
    background-color: #ffffff;
    box-shadow: inset 0 0 0 1px rgba(15, 24, 39, 0.20);
  }

  &:disabled {
    opacity: 0.6;
    cursor: not-allowed;
  }
`;

const Input = styled.input`
  ${inputStyles}
`;

const Select = styled.select`
  ${inputStyles}
  appearance: none;
  cursor: pointer;
`;

const Actions = styled.div`
  margin-top: 3rem;
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
`;

const CancelBtn = styled.button`
  background: none;
  border: none;
  color: ${({ theme }) => theme.colors.secondary};
  font-weight: 600;
  font-size: 0.875rem;
  padding: 0.75rem 1.5rem;
  cursor: pointer;

  &:hover {
    color: ${({ theme }) => theme.colors.primary};
  }
`;

const SubmitBtn = styled.button`
  background-color: ${({ theme }) => theme.colors.tertiaryContainer};
  color: ${({ theme }) => theme.colors.onTertiaryContainer};
  font-weight: 800;
  font-family: ${({ theme }) => theme.typography.display};
  padding: 0.75rem 2rem;
  border-radius: 0.5rem;
  border: none;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  cursor: pointer;
  transition: all 0.2s;

  span {
    font-size: 18px;
    font-variation-settings: 'FILL' 1;
  }

  &:hover {
    opacity: 0.9;
  }

  &:disabled {
    opacity: 0.7;
    cursor: not-allowed;
  }
`;

const LoadingState = styled.div`
  min-height: 50vh;
  display: flex;
  align-items: center;
  justify-content: center;
  color: ${({ theme }) => theme.colors.secondary};
  font-weight: 600;
`;
