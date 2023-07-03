import { Routes, Route } from 'react-router-dom'
import './App.css'
import { Home } from './pages'
import { LayoutContainer } from './styled-components'
import { ApplicationBar } from './views'
import { SignIn } from './pages/SignIn'
import { SignUp } from './pages/SignUp'
import { RequireAuth } from 'react-auth-kit';

function App() {
  return (
    <>
      <LayoutContainer>
      <ApplicationBar />
      <Routes>
        <Route path="/" element={(
          <RequireAuth loginPath="/sign-in">
            <Home/>
        </RequireAuth>)}/>
        <Route path="/sign-in" element={<SignIn/>}/>
        <Route path="/sign-up" element={<SignUp/>}/>
      </Routes>
      </LayoutContainer>
    </>
  )
}

export default App
