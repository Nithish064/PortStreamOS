import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "PortStream OS",
  description: "Next-Generation Port Management System",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body className="antialiased bg-[#0f172a] text-white">
        <nav className="glass-panel sticky top-0 z-50 flex items-center justify-between px-6 py-4 mb-8">
          <div className="flex items-center gap-3">
            <div className="w-8 h-8 rounded-full bg-sky-400 animate-pulse-slow" />
            <h1 className="text-xl font-bold tracking-wider text-sky-400">PortStream<span className="text-white">OS</span></h1>
          </div>
          <div className="flex gap-4 text-sm font-medium text-slate-300">
            <a href="/" className="hover:text-sky-400 transition-colors">Dashboard</a>
            <a href="/yard" className="hover:text-sky-400 transition-colors">Yard</a>
            <a href="/customs" className="hover:text-sky-400 transition-colors">Customs</a>
            <a href="/gate" className="hover:text-sky-400 transition-colors">Gate & Billing</a>
            <a href="/operations" className="hover:text-sky-400 transition-colors">Operations</a>
          </div>
        </nav>
        <main className="container mx-auto px-4 pb-12">
          {children}
        </main>
      </body>
    </html>
  );
}
