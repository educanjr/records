import * as React from 'react';
import Container from '@mui/material/Container';
import Box from '@mui/material/Box';

export default function ApplicationForm(props: React.HTMLAttributes<HTMLDivElement>) {
  const { children } = props;

  return (
    <Box
      sx={{
        display: 'flex',
        backgroundRepeat: 'no-repeat',
      }}
    >
      <Container maxWidth="sm">
        <Box sx={{ mt: 7, mb: 12 }}>
            {children}
        </Box>
      </Container>
    </Box>
  );
}
