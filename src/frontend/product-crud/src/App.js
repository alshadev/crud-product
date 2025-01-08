import React, { useState } from 'react';
import './App.css';
import LoginForm from './components/LoginForm';
import RegisterForm from './components/RegisterForm';
import ProductTable from './components/ProductTable';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [token, setToken] = useState(null);
  const [error, setError] = useState('');

  const handleLogin = (token) => {
    setIsLoggedIn(true);
    setToken(token);
    setError('');
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
    setToken(null);
  };

  const handleRegisterSuccess = () => {
    setError('');
    alert('Registration successful! Please log in.');
  };

  const handleError = (message) => {
    setError(message);
  };

  return (
    <div className="App">
      <h1>Product CRUD</h1>

      {isLoggedIn ? (
        <>
          <button onClick={handleLogout}>Logout</button>
          <ProductTable token={token} />
        </>
      ) : (
        <>
          <LoginForm onLogin={handleLogin} onError={handleError} />
          <RegisterForm onRegister={handleRegisterSuccess} onError={handleError} />
        </>
      )}

      {error && <div className="error">{error}</div>}
    </div>
  );
}

export default App;