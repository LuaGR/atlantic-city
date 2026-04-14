'use client';

import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { useRouter } from 'next/navigation';
import { PedidoService, PedidoDto } from '@/modules/pedidos/infrastructure/pedido.service';

export default function PedidosPage() {
  const router = useRouter();
  const [pedidos, setPedidos] = useState<PedidoDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchPedidos();
  }, []);

  const fetchPedidos = async () => {
    try {
      setLoading(true);
      const data = await PedidoService.getAll();
      setPedidos(data);
      setError(null);
    } catch (err) {
      setError('Error al cargar los pedidos. Intente nuevamente.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('¿Está seguro de eliminar este pedido?')) return;
    
    try {
      await PedidoService.delete(id);
      setPedidos(pedidos.filter((p) => p.id !== id));
    } catch (err) {
      alert('Error al eliminar el pedido.');
      console.error(err);
    }
  };

  const getStatusColor = (estado: string) => {
    switch (estado) {
      case 'Entregado': return '#00ca65';
      case 'Pendiente': return '#f39c12';
      case 'En Proceso': return '#3498db';
      case 'Cancelado': return '#e74c3c';
      default: return '#7f8c8d';
    }
  };

  return (
    <PageContainer>
      <Header>
        <Title>Listado de Pedidos</Title>
        <CreateBtn onClick={() => router.push('/pedidos/crear')}>
          <span className="material-symbols-outlined">add</span>
          Crear Pedido
        </CreateBtn>
      </Header>
      
      {error && <ErrorMessage>{error}</ErrorMessage>}

      {loading ? (
        <LoadingState>Cargando pedidos...</LoadingState>
      ) : pedidos.length === 0 ? (
        <EmptyState>
          <span className="material-symbols-outlined">inbox</span>
          <p>Aún no hay pedidos listados.</p>
        </EmptyState>
      ) : (
        <TableContainer>
          <Table>
            <Thead>
              <Tr>
                <Th>ID</Th>
                <Th>Nro. Referencia</Th>
                <Th>Cliente</Th>
                <Th>Total</Th>
                <Th>Estado</Th>
                <Th>Acciones</Th>
              </Tr>
            </Thead>
            <Tbody>
              {pedidos.map((pedido) => (
                <Tr key={pedido.id}>
                  <Td>{pedido.id}</Td>
                  <Td><strong>{pedido.numeroPedido}</strong></Td>
                  <Td>{pedido.cliente}</Td>
                  <Td>${pedido.total.toFixed(2)}</Td>
                  <Td>
                    <StatusBadge $color={getStatusColor(pedido.estado)}>
                      {pedido.estado}
                    </StatusBadge>
                  </Td>
                  <Td>
                    <ActionGroup>
                      <ActionBtn 
                        className="material-symbols-outlined" 
                        title="Editar"
                        onClick={() => router.push(`/pedidos/editar/${pedido.id}`)}
                      >
                        edit
                      </ActionBtn>
                      <ActionBtn 
                        className="material-symbols-outlined delete" 
                        title="Eliminar"
                        onClick={() => handleDelete(pedido.id)}
                      >
                        delete
                      </ActionBtn>
                    </ActionGroup>
                  </Td>
                </Tr>
              ))}
            </Tbody>
          </Table>
        </TableContainer>
      )}
    </PageContainer>
  );
}

const PageContainer = styled.div`
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
`;

const Header = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
`;

const Title = styled.h1`
  font-size: 2rem;
  font-weight: 800;
  color: ${({ theme }) => theme.colors.primary};
`;

const CreateBtn = styled.button`
  background-color: ${({ theme }) => theme.colors.tertiaryContainer};
  color: ${({ theme }) => theme.colors.onTertiaryContainer};
  padding: 0.5rem 1.25rem;
  border-radius: 0.5rem;
  border: none;
  font-weight: 700;
  font-family: inherit;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  cursor: pointer;

  span {
    font-size: 18px;
  }

  &:hover {
    opacity: 0.9;
  }
`;

const ErrorMessage = styled.div`
  background-color: ${({ theme }) => theme.colors.error}15;
  color: ${({ theme }) => theme.colors.error};
  padding: 1rem;
  border-radius: 8px;
  margin-bottom: 1.5rem;
  font-weight: 500;
`;

const LoadingState = styled.div`
  text-align: center;
  padding: 4rem;
  color: ${({ theme }) => theme.colors.secondary};
  font-weight: 500;
`;

const EmptyState = styled.div`
  background-color: ${({ theme }) => theme.colors.surfaceContainerLowest};
  border-radius: 12px;
  padding: 4rem 2rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
  color: ${({ theme }) => theme.colors.onSurfaceVariant};

  span {
    font-size: 48px;
    opacity: 0.5;
  }

  p {
    font-weight: 600;
  }
`;

const TableContainer = styled.div`
  background-color: ${({ theme }) => theme.colors.surfaceContainerLowest};
  border-radius: 12px;
  box-shadow: ${({ theme }) => theme.shadows.cloud};
  overflow: hidden;
`;

const Table = styled.table`
  width: 100%;
  border-collapse: collapse;
  text-align: left;
`;

const Thead = styled.thead`
  background-color: ${({ theme }) => theme.colors.surfaceContainerLow};
`;

const Tbody = styled.tbody`
  & tr:last-child td {
    border-bottom: none;
  }
`;

const Tr = styled.tr`
  transition: background-color 0.2s;
  &:hover {
    background-color: ${({ theme }) => theme.colors.surfaceContainer};
  }
`;

const Th = styled.th`
  padding: 1rem 1.5rem;
  font-size: 0.75rem;
  font-weight: 700;
  color: ${({ theme }) => theme.colors.secondary};
  text-transform: uppercase;
  letter-spacing: 0.05em;
`;

const Td = styled.td`
  padding: 1rem 1.5rem;
  border-bottom: 1px solid ${({ theme }) => theme.colors.surfaceContainer};
  font-size: 0.875rem;
  color: ${({ theme }) => theme.colors.primary};
`;

const StatusBadge = styled.span<{ $color: string }>`
  background-color: ${({ $color }) => $color}15;
  color: ${({ $color }) => $color};
  padding: 0.25rem 0.75rem;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 700;
  text-transform: uppercase;
`;

const ActionGroup = styled.div`
  display: flex;
  gap: 0.5rem;
`;

const ActionBtn = styled.button`
  background: none;
  border: none;
  color: ${({ theme }) => theme.colors.secondary};
  cursor: pointer;
  padding: 0.25rem;
  border-radius: 4px;
  transition: all 0.2s;

  &:hover {
    color: ${({ theme }) => theme.colors.primary};
    background-color: ${({ theme }) => theme.colors.surfaceContainerHigh};
  }

  &.delete:hover {
    color: ${({ theme }) => theme.colors.error};
    background-color: ${({ theme }) => theme.colors.error}15;
  }
`;
