import apiClient from './client';
import { jwtDecode } from 'jwt-decode';


interface LoginResponse {
  token: string;
}

export interface LoginDTO {
  userId: string;
  password: string;
}

export const login = async (dto: LoginDTO) => {
  try {
   const response = await apiClient.post<LoginResponse>('/auth/login', dto);

   // 解析 Token
   const token = response.data.token;
   const decodedToken = jwtDecode(token);
   console.log(decodedToken); // 打印 Token 內容
   localStorage.setItem('token', token);

   return {message : '',result:true,token:response.data.token};
  } catch (error) {
    console.error("Login failed", error);
    // 處理登入失敗
    return {message : 'Login failed. Please check your credentials.',result:false};
  }
};
