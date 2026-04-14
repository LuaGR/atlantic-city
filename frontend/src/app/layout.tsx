import type { Metadata } from "next";
import Providers from "@/lib/Providers";

export const metadata: Metadata = {
  title: "Atlantic City - Administration",
  description: "Enterprise Administration Portal for Atlantic City",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="es">
      <head>
        <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&display=swap" rel="stylesheet" />
      </head>
      <body>
        <Providers>{children}</Providers>
      </body>
    </html>
  );
}
