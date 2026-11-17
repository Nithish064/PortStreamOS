export const fetchVessels = async () => {
  try {
    const res = await fetch('http://localhost:5000/api/vessels', { cache: 'no-store' });
    if (!res.ok) throw new Error('Failed to fetch vessels');
    return await res.json();
  } catch (error) {
    console.error(error);
    return []; // Return empty array on failure for now
  }
};
