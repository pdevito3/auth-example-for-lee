import PrivateLayout from "@/components/PrivateLayout";
import Head from "next/head";

export default function Home() {
  return (
    <>
      {/* TODO head update */}
      <Head>
        <title>Create Next App</title>
        <link rel="icon" href="/favicon.svg" />
      </Head>

      <PrivateLayout>home</PrivateLayout>
    </>
  );
}
