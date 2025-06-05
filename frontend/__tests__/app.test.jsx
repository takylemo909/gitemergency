import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import App from '../app.jsx';

global.fetch = jest.fn(() =>
  Promise.resolve({
    text: () => Promise.resolve('Welcome to the Simple Shop API!'),
  })
);

test('renders welcome message from API', async () => {
  render(<App />);
  await waitFor(() => {
    expect(screen.getByText('Welcome to the Simple Shop API!')).toBeInTheDocument();
  });
});
