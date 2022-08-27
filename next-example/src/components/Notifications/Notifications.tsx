import { Toaster } from "react-hot-toast";

const Notifications = () => {
  return (
    <Toaster
      position="top-right"
      gutter={6}
      containerClassName=""
      containerStyle={{}}
      toastOptions={{
        // Define default options
        className: "",
        duration: 5000,
        style: {
          background: "#363636",
          color: "#fff",
        },

        // Default options for specific types
        success: {
          duration: 2000,
          theme: {
            primary: "green",
            secondary: "black",
          },
        },
      }}
    />
  );
};

// Notifications.success = (message: string, options?: ToastOptions<{}>) => {
// 	toast.success(
// 		<div className='mx-2'>{message}</div>,
// 		Object.assign(
// 			{
// 				bodyClassName: 'py-3',
// 			},
// 			options
// 		)
// 	);
// };

// Notifications.error = (message: string, options?: ToastOptions<{}>) => {
// 	toast.error(
// 		<div className='mx-2'>{message}</div>,
// 		Object.assign(
// 			{
// 				bodyClassName: 'py-3',
// 			},
// 			options
// 		)
// 	);
// };

export { Notifications };
