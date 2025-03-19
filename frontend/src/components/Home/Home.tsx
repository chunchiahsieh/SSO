
import {
  Box,
} from '@mui/material';

export interface HomeProps {
  prop?: string;
}

export function Home({prop = 'default value'}: HomeProps) {
  return (
    <Box sx={{ display: 'flex', height: '100vh' }}>
      {/* 左側選單 */}
      <Box sx={{ width: 200,  p: 1, overflowY: 'auto' , border: '1px solid #ccc'}}>

       {/* 在左上角加入一個 IconButton 用來切換右側選單狀態 */}
       <Box sx={{ display: 'flex', justifyContent: 'flex-end', mb: 1 }}>
            
          </Box>

      </Box>


      {/* 右側內容區 */}
      <Box sx={{ flex: 1, display: 'flex', flexDirection: 'column',border: '0px solid #ccc', }}>
        
      {/* 內容區，貼合在 Tabs 下方 */}
        <Box 
        sx={{
          flex: 1,
          p: 2,
          overflowY: 'auto',
          border: '1px solid #ccc',
          borderTop: 'none', // 移除上邊框，讓內容區與 Tabs 貼合
          marginTop: 0,
        }}
        >
          
        </Box>
      </Box>
    </Box>
  );
};

