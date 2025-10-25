'use client';
import { useState } from 'react';

export default function Home() {
  const [longUrl, setLongUrl] = useState('');
  const [shortUrl, setShortUrl] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const apiUrl = process.env.NEXT_PUBLIC_API_URL;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setShortUrl('');
    setIsLoading(true);

    if (!apiUrl) {
      setError("Erro de configuração: URL da API não definida.");
      setIsLoading(false);
      return;
    }

    try {
      const response = await fetch(`${apiUrl}/api/shorten`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ url: longUrl }),
      });

      if (!response.ok) {
        // Tenta ler a mensagem de erro da validação
        const errorData = await response.json();
        const errorMessage = errorData.errors?.Url[0] || "Falha ao encurtar. A URL é válida?";
        throw new Error(errorMessage);
      }

      const data = await response.json();
      setShortUrl(data.shortUrl);
    } catch (err: any) {
      setError(err.message || 'Erro de conexão. O backend está rodando?');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <main className="flex min-h-screen flex-col items-center justify-center p-24 bg-gray-900 text-white">
      <div className="w-full max-w-md p-8 bg-gray-800 rounded-lg shadow-xl">
        <h1 className="text-3xl font-bold text-center mb-6">Encurtador de URL</h1>

        <form onSubmit={handleSubmit}>
          <label htmlFor="url-input" className="block text-sm font-medium text-gray-300 mb-2">
            Cole sua URL longa:
          </label>
          <input
            id="url-input"
            type="text"
            value={longUrl}
            onChange={(e) => setLongUrl(e.target.value)}
            placeholder="https://exemplo-de-url-muito-longa.com/..."
            className="w-full p-3 bg-gray-700 border border-gray-600 rounded-md text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
            disabled={isLoading}
          />
          <button
            type="submit"
            className="w-full p-3 mt-4 bg-blue-600 rounded-md font-semibold hover:bg-blue-700 transition-colors disabled:bg-gray-500"
            disabled={isLoading}
          >
            {isLoading ? "Encurtando..." : "Encurtar!"}
          </button>
        </form>

        {shortUrl && (
          <div className="mt-6 p-4 bg-gray-700 rounded-md text-center">
            <p className="text-sm text-gray-300">Sua URL curta:</p>
            <a
              href={shortUrl}
              target="_blank"
              rel="noopener noreferrer"
              className="text-lg font-medium text-blue-400 hover:underline"
            >
              {shortUrl}
            </a>
          </div>
        )}

        {error && (
          <div className="mt-6 p-4 bg-red-900 border border-red-700 rounded-md text-center">
            <p className="text-sm text-red-300">{error}</p>
          </div>
        )}
      </div>
    </main>
  );
}