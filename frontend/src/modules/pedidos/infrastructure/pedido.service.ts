import { api } from '@/core/infrastructure/axios';

export enum EstadoPedido {
  Registrado = 'Registrado',
  Procesado = 'Procesado',
  Enviado = 'Enviado',
  Entregado = 'Entregado',
  Cancelado = 'Cancelado'
}

export interface PedidoDto {
  id: number;
  numeroPedido: string;
  cliente: string;
  fecha: string;
  total: number;
  estado: EstadoPedido;
}

export interface CreatePedidoDto {
  numeroPedido: string;
  cliente: string;
  fecha: string;
  total: number;
  estado: string;
}

export interface UpdatePedidoDto {
  cliente: string;
  fecha: string;
  total: number;
  estado: string;
}

export const PedidoService = {
  async getAll(): Promise<PedidoDto[]> {
    const response = await api.get<PedidoDto[]>('/api/pedidos');
    return response.data;
  },

  async getById(id: number): Promise<PedidoDto> {
    const response = await api.get<PedidoDto>(`/api/pedidos/${id}`);
    return response.data;
  },

  async create(datos: CreatePedidoDto): Promise<PedidoDto> {
    const response = await api.post<PedidoDto>('/api/pedidos', datos);
    return response.data;
  },

  async update(id: number, datos: UpdatePedidoDto): Promise<PedidoDto> {
    const response = await api.put<PedidoDto>(`/api/pedidos/${id}`, datos);
    return response.data;
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/api/pedidos/${id}`);
  }
};
