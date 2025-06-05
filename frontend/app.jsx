function App() {
  const [message, setMessage] = React.useState('');

  React.useEffect(() => {
    fetch('http://localhost:5000/', {
      headers: {
        'Authorization': 'Basic ' + btoa('admin:password')
      }
    })
    .then(res => res.text())
    .then(setMessage)
    .catch(err => setMessage('Error: ' + err));
  }, []);

  return (
    <div>
      <h1>Simple Shop</h1>
      <p>{message}</p>
    </div>
  );
}

ReactDOM.render(<App />, document.getElementById('root'));
if (typeof module !== 'undefined') {
  module.exports = App;
}
