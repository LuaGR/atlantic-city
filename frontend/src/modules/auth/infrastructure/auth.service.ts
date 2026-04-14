import { api } from '@/core/infrastructure/axios';

export interface LoginResponseDto {
  token: string;
  expiresIn: number;
}

export const AuthService = {
  async login(email: string, password: string): Promise<LoginResponseDto> {
    const response = await api.post<LoginResponseDto>('/auth/login', { email, password });
    return response.data;
  }
};
