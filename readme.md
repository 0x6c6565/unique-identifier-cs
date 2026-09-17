# Unique Identifiers

Serializable guids for Unity

## Usage

Add the package from github url

```C#
using UniqueIdentifiers;

// Declare a type to use.
public Guid64 myGuid64 = Guid64.NewGuid64();

// Or...
public Guid128 myGuid128 = Guid128.NewGuid128();

```

In the Unity inspector, right clicking on the identifier allows the following options:
- Make empty (sets the guid to the value of `Guid64.Empty()|Guid128.NewGuid128()`.)
- Create New (assigns a new unique identifier, using `Guid64.NewGuid64()|Guid128.NewGuid128()`.)
- Copy (copies the current unique identifier to the clipboard.)
- Pasta (pastes the value from the clipboard into the current object.)

## Licensing

MIT License

Copyright 2026 lee wood

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

-------------

Heavily influenced by Twitter Snowflake

Copyright 2010-2012 Twitter, Inc.

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this 
file except in compliance with the License. You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed 
under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied. See the License for the 
specific language governing permissions and limitations under the License.