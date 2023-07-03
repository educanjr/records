import * as React from 'react';
import { Field, Form, FormSpy } from 'react-final-form';
import Box from '@mui/material/Box';
import Link from '@mui/material/Link';
import Typography from '@mui/material/Typography';
import AppAppBar from '@/views/ApplicationBar/ApplicationBar';
import AppForm from '@/views/ApplicationForm/ApplicationForm';
import { email, required } from '@/components/Form/validation';

import FormButton from '@/components/Form/FormButton';
import RFTextField from '@/components/Form/RFTextField';
import FormFeedback from '@/components/Form/FormFeedback';
import axios, {AxiosError} from 'axios';
import {useSignIn} from 'react-auth-kit';


function SignIn() {
  const [sent, setSent] = React.useState(false);

  const signIn = useSignIn();

  const validate = (values: { [index: string]: string }) => {
    const errors = required(['email', 'password'], values);

    if (!errors.email) {
      const emailError = email(values.email);
      if (emailError) {
        errors.email = emailError;
      }
    }

    return errors;
  };

  const handleSubmit = async (values: any) => {
    console.log(values);
    setSent(true);
    try{
      const response = await axios.post(
        "http://localhost:7006/api/Users/login",
        values
      );

      signIn({
        token: response.data,
        expiresIn: 3600,
        tokenType: "Bearer",
        authState: { email: values.email },
      });
    } catch(err){
      if (err && err instanceof AxiosError){
        console.log("Axios Internal Error");
      }
      else {
        console.log(err);
      }
    }
  };

  return (
    <React.Fragment>
      <AppAppBar />
      <AppForm>
        <React.Fragment>
          <Typography variant='h3' gutterBottom align='center'>
            Sign In
          </Typography>
          <Typography variant="body2" align="center">
            {'Not a member yet? '}
            <Link
              href="/sign-up"
              align="center"
              underline="always"
            >
              Sign Up here
            </Link>
          </Typography>
        </React.Fragment>
        <Form
          onSubmit={handleSubmit}
          subscription={{ submitting: true }}
          validate={validate}
        >
          {({ handleSubmit: handleSubmit2, submitting }) => (
            <Box component="form" onSubmit={handleSubmit2} noValidate sx={{ mt: 6 }}>
              <Field
                autoComplete="email"
                autoFocus
                component={RFTextField}
                disabled={submitting || sent}
                fullWidth
                label="Email"
                margin="normal"
                name="email"
                required
              />
              <Field
                fullWidth
                component={RFTextField}
                disabled={submitting || sent}
                required
                name="password"
                autoComplete="current-password"
                label="Password"
                type="password"
                margin="normal"
              />
              <FormSpy subscription={{ submitError: true }}>
                {({ submitError }) =>
                  submitError ? (
                    <FormFeedback error sx={{ mt: 2 }}>
                      {submitError}
                    </FormFeedback>
                  ) : null
                }
              </FormSpy>
              <FormButton
                sx={{ mt: 3, mb: 2 }}
                disabled={submitting || sent}
                size="large"
                color="secondary"
                fullWidth
              >
                {submitting || sent ? 'In progress…' : 'Sign In'}
              </FormButton>
            </Box>
          )}
        </Form>
      </AppForm>
    </React.Fragment>
  );
}

export default SignIn;
