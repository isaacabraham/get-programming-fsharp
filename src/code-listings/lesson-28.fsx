module OrderManagement

open System.Collections.Generic

// Listing 28.1
type OrderItemRequest = { ItemId : int; Count : int }
type OrderRequest =
    { OrderId : int
      CustomerName : string // mandatory
      Comment : string // optional
      /// Only one of email and telephone should be populated, or none
      EmailUpdates : string
      TelephoneUpdates : string
      Items : IEnumerable<OrderItemRequest> } // mandatory

module private OrderProcessing =
    // Listing 28.2
    type OrderId = OrderId of int
    type ItemId = ItemId of int
    type OrderItem = { ItemId : ItemId; Count : int }
    type UpdatePreference = | EmailUpdates of string | TelephoneUpdates of string
    type Order =
        { OrderId : OrderId
          CustomerName : string
          ContactPreference : UpdatePreference option
          Comment : string option
          Items : OrderItem list }
    // Listing 28.3
    let toOrder (orderRequest:OrderRequest) =
        { OrderId = OrderId orderRequest.OrderId
          CustomerName =
            match orderRequest.CustomerName with
            | null -> failwith "Customer name must be populated"
            | name -> name
          Comment = orderRequest.Comment |> Option.ofObj
          ContactPreference =
            match Option.ofObj orderRequest.EmailUpdates, Option.ofObj orderRequest.TelephoneUpdates  with
            | None, None -> None
            | Some email, None -> Some(EmailUpdates email)
            | None, Some phone -> Some(TelephoneUpdates phone)
            | Some _, Some _ -> failwith "Unable to proceed - only one of telephone and email should be supplied"
          Items =
            orderRequest.Items
            |> Seq.map(fun item -> { ItemId = ItemId item.ItemId; Count = item.Count })
            |> Seq.toList }
    
    let processOrder (order:Order) = "SUCCESS"

let placeOrder = OrderProcessing.toOrder >> OrderProcessing.processOrder 

