### Design Considerations

- First of all, I'm considering 2 main non-functional requirements to be security and low latency.
- I'm trying to find a balance between fulfilling those requirements and not over-engineering the exercise.
    - I've opted to add just the default logging and resiliency patterns, just to show how I'd start them in real life.
- The model is pretty simple, and given the performance requirements, I've opted for writing my own validation logic,
  since most of the ComponentModel validation attributes depend on reflection. I've limited my use of them where
  necessary for the Swagger documentation.
    - For object mapping, however, there is Mapperly, which is a source generating adapter
      and as such doesn't incur in the reflection performante penalties.
    - Same can be said for Refit. It generates sources at compile-time, so the performance is the same as a hand-coded HttpClient. 
     Also, it's plugged in HttpClientFactory, so we get HttpMessageHandler lifecycle management for free. 
- This exercise sent me into a nice rabbit hole on how to properly secure strings in .NET.
  I already knew the SecureString class was not considered secure anymore,
  since any attacker would have access to the memory anyway.
  Still, I believe this kind of application should strive to make the sensitive data as short-lived as possible,
  not only because of possible security breaches, but to avoid it appearing in any log, trace or memory dump.
  I'm aware logging and tracing can be configured to redact these, but still...better safe than sorry.
  What I've found out is how hard it is to be certain a string is deleted with the way strings are managed by the runtime. 
  The GC can compact the heap, move them around, and there's no guarantee of when they'll be collected.
  One alternative would be to force a collection everytime a credit card number went out of scope, which isn't feasible.
  My conclusion was that the bytes of the request Stream representing the sensitive data should be read directly into a
  pinned buffer,
  (like a Span\<T>), which could then be manually zeroed. I started to try and implement it, but realized it would be
  over-engineering for this exercise.
- I'm not really familiar with the Domain language of payment gateways, so I struggled a bit to name the entities.
  I also checked your API reference docs for inspiration, and the real world is way more complicated :) 
  I didn't know if what I received was a payment submission, request or order. Same with what I sent to the bank. No big
  deal though, just registering my thoughts.
