import React, { useState } from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Paper from '@mui/material/Paper';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { useNavigate ,useLocation } from 'react-router-dom';
import {login,LoginDTO} from '../../Api/auth';

const theme = createTheme();

export function Login() {
  const [errorMessage, setErrorMessage] = useState<string>(''); // 這裡要設置初始值，默認為空字串
  const navigate = useNavigate(); // 使用 useNavigate


  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    // 使用 LoginDTO 物件來接收資料
    const loginData: LoginDTO = {
      userId: String(data.get('userid')), // 強制轉換為字串
      password: String(data.get('password')), // 強制轉換為字串
    };
    console.log(loginData);
     // 呼叫 login 函數並處理登入邏輯

     const {message, result,token } = await login(loginData);

       // **取得 `redirectUrl`，如果沒有則預設跳轉 `/home`**
    const redirectUrl = new URLSearchParams(location.search).get("redirectUrl") || "/home";

     if (result == true) {
      if (redirectUrl.startsWith("http")) {
        const url = token ? `${redirectUrl}?token=${encodeURIComponent(token)}` : redirectUrl;
        window.location.href = url;
      } else {
        navigate(redirectUrl);
      }
     } else {
       // 登入失敗，顯示錯誤訊息
       setErrorMessage(message);
     }
  };

  return (
    <ThemeProvider theme={theme}>
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center', // 水平居中
          alignItems: 'center', // 垂直居中
          height: '100vh', // 高度設為100%視窗高度
          width: '100%', // 保證寬度覆蓋整個視窗
        }}
      >
        <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 400 }}>
          <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
            <LockOutlinedIcon />
          </Avatar>
          <Typography component="h1" variant="h5">
            Sign in
          </Typography>
          <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 1 }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="userid"
              label="User Id"
              name="userid"
              autoComplete="UserId"
              autoFocus
            />
            <TextField
              margin="normal"
              required
              fullWidth
              name="password"
              label="Password"
              type="password"
              id="password"
              autoComplete="current-password"
            />
            <FormControlLabel
              control={<Checkbox value="remember" color="primary" />}
              label="Remember me"
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Sign In
            </Button>
            {errorMessage && (
              <div style={{ color: 'red', fontSize: '16px', fontWeight: 'bold' }}>
                {errorMessage}
              </div>
            )}
          </Box>
        </Paper>
      </Box>
    </ThemeProvider>
  );
}

export default Login;
