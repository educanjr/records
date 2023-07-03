import MuiAppBar, { AppBarProps } from '@mui/material/AppBar';

function NavBar(props: AppBarProps) {
  return <MuiAppBar elevation={0} position="fixed" {...props} />;
}

export default NavBar;
