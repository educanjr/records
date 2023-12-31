import * as React from 'react';
import { ButtonProps } from '@mui/material';
import MuiButton from '@mui/material/Button';
import defer from './defer';

interface FormButtonProps {
  disabled?: boolean;
  mounted?: boolean;
}

function FormButton<C extends React.ElementType>(
  props: FormButtonProps & ButtonProps<C, { component?: C }>,
) {
  const { disabled, mounted, ...others } = props;
  return (
    <MuiButton
      disabled={!mounted || !!disabled}
      type="submit"
      variant="contained"
      {...others}
    />
  );
}
export default defer(FormButton);
