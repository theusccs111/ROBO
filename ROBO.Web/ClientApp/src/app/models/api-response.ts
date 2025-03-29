export default interface ApiResponse<TData>
{
  success?: boolean,
  message?: string,
  message_detail?: string,
  data?: TData | undefined
}
