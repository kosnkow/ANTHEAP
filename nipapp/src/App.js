import React, { useState } from 'react';
import {variables} from './variable.js';

function App() {
  const [nip, setNip] = useState('');
  const [result, setResult] = useState(null);
  const [noDataMessage, setNoDataMessage] = useState('');

  const handleSearch = () => {
    fetch(variables.API_URL$ + nip)
      .then(response => response.json())
      .then(data => {
        if (data.nip === 0) {
          setNoDataMessage('No nip data available');
        } else {
          setResult(data)
        }  
      });
  };

  return (
    <div className="container">
      <h1>NIP Details</h1>
      <input
        type="text"
        value={nip}
        onChange={event => setNip(event.target.value)}
        placeholder="Enter NIP"
      />
      <button onClick={handleSearch}>Search</button>
      {result && (
        <div id="resultContainer">
          <h2>Result:</h2>
          <p>Name: {result.name}</p>
          <p>Nip: {result.nip}</p>
          <p>Status Vat: {result.statusVat}</p>
          <p>Regon: {result.regon}</p>
          <p>Pesel: {result.pesel}</p>
          <p>Registration Legal Date: {result.registrationLegalDate}</p>
          <p>regon: {result.regon}</p>
        </div>
      )}
      {noDataMessage && <p>{noDataMessage}</p>}
      {result && (
          <div id="accountContainer">
          <h2>Account List:</h2>
          <ul>
            {result.accountList.length > 0 && result.accountList.map(account => (
              <li>{account}</li>
            ))}
          </ul>
          </div>
      )}
      
    </div>
  );
}

export default App;